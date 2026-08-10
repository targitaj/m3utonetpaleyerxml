using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using Newtonsoft.Json;

namespace MediaNotes.LegacyApi
{
    internal static class NotesStore
    {
        private static readonly object InitLock = new object();
        private static readonly Timer SnapshotTimer;
        private static bool _initialized;
        private const string Iso = "o";
        private const string StatsAdminEmail = "mosala@gmail.com";

        static NotesStore()
        {
            SnapshotTimer = new Timer(_ => RunDailySnapshots(), null,
                TimeSpan.FromMinutes(2), TimeSpan.FromHours(6));
        }

        private static string DatabasePath
        {
            get
            {
                string root = null;
                try { root = HttpRuntime.AppDomainAppPath; }
                catch (ArgumentNullException) { }
                if (string.IsNullOrWhiteSpace(root)) root = AppDomain.CurrentDomain.BaseDirectory;
                return Path.Combine(root, "App_Data", "medianotes.db");
            }
        }

        private static SQLiteConnection Open()
        {
            EnsureInitialized();
            var connection = new SQLiteConnection("Data Source=" + DatabasePath + ";Version=3;Foreign Keys=True;");
            connection.Open();
            return connection;
        }

        private static void EnsureInitialized()
        {
            if (_initialized) return;
            lock (InitLock)
            {
                if (_initialized) return;
                Directory.CreateDirectory(Path.GetDirectoryName(DatabasePath));
                using (var connection = new SQLiteConnection("Data Source=" + DatabasePath + ";Version=3;"))
                {
                    connection.Open();
                    Execute(connection, @"
CREATE TABLE IF NOT EXISTS Users (
 Id TEXT PRIMARY KEY, Email TEXT NOT NULL UNIQUE, PasswordHash BLOB NOT NULL,
 PasswordSalt BLOB NOT NULL, CreatedUtc TEXT NOT NULL);
CREATE TABLE IF NOT EXISTS Sessions (
 TokenHash TEXT PRIMARY KEY, UserId TEXT NOT NULL, ExpiresUtc TEXT NOT NULL);
CREATE TABLE IF NOT EXISTS UserState (UserId TEXT PRIMARY KEY, SyncVersion INTEGER NOT NULL);
CREATE TABLE IF NOT EXISTS Notes (
 Id TEXT NOT NULL, UserId TEXT NOT NULL, Text TEXT NOT NULL, Color TEXT NOT NULL,
 CreatedUtc TEXT NOT NULL, ModifiedUtc TEXT NOT NULL, IsDeleted INTEGER NOT NULL,
 Version INTEGER NOT NULL, PRIMARY KEY (Id, UserId));
CREATE INDEX IF NOT EXISTS IX_Notes_User_Version ON Notes(UserId, Version);
CREATE TABLE IF NOT EXISTS ImageBlobs (
 UserId TEXT NOT NULL, Hash TEXT NOT NULL, MimeType TEXT NOT NULL, Data BLOB NOT NULL,
 PRIMARY KEY (UserId, Hash));
CREATE TABLE IF NOT EXISTS NoteImages (
 UserId TEXT NOT NULL, NoteId TEXT NOT NULL, Hash TEXT NOT NULL, SortOrder INTEGER NOT NULL,
 PRIMARY KEY (UserId, NoteId, Hash));
CREATE TABLE IF NOT EXISTS Revisions (
 Id TEXT PRIMARY KEY, UserId TEXT NOT NULL, ContentHash TEXT NOT NULL,
 Text TEXT NOT NULL, Color TEXT NOT NULL, ImageHashesJson TEXT NOT NULL,
 FirstSeenUtc TEXT NOT NULL, UNIQUE(UserId, ContentHash));
CREATE TABLE IF NOT EXISTS Snapshots (
 Id TEXT PRIMARY KEY, UserId TEXT NOT NULL, SnapshotDate TEXT NOT NULL,
 CreatedUtc TEXT NOT NULL, UNIQUE(UserId, SnapshotDate));
CREATE TABLE IF NOT EXISTS SnapshotDeletions (
 UserId TEXT NOT NULL, SnapshotDate TEXT NOT NULL, PRIMARY KEY(UserId, SnapshotDate));
CREATE TABLE IF NOT EXISTS SnapshotItems (
 SnapshotId TEXT NOT NULL, NoteId TEXT NOT NULL, RevisionId TEXT NOT NULL,
 PRIMARY KEY (SnapshotId, NoteId));
CREATE TABLE IF NOT EXISTS NoteShares (
 Token TEXT PRIMARY KEY, OwnerUserId TEXT NOT NULL, NoteId TEXT NOT NULL,
 CreatedUtc TEXT NOT NULL, IsRevoked INTEGER NOT NULL DEFAULT 0,
 UNIQUE(OwnerUserId, NoteId));
CREATE TABLE IF NOT EXISTS SharedNoteSubscriptions (
 OwnerUserId TEXT NOT NULL, NoteId TEXT NOT NULL, RecipientUserId TEXT NOT NULL,
 Version INTEGER NOT NULL, IsRemoved INTEGER NOT NULL DEFAULT 0,
 CreatedUtc TEXT NOT NULL,
 PRIMARY KEY (OwnerUserId, NoteId, RecipientUserId));
CREATE INDEX IF NOT EXISTS IX_SharedNotes_Recipient_Version
 ON SharedNoteSubscriptions(RecipientUserId, Version);");
                }
                _initialized = true;
            }
        }

        private static void RunDailySnapshots()
        {
            try
            {
                var users = new List<string>();
                using (var connection = Open())
                using (var command = Command(connection, "SELECT Id FROM Users"))
                using (var reader = command.ExecuteReader())
                    while (reader.Read()) users.Add(reader.GetString(0));
                foreach (var userId in users) EnsureSnapshot(userId, DateTime.UtcNow.Date);
            }
            catch
            {
                // The next six-hour pass retries; API requests continue to work independently.
            }
        }

        public static AdminStatsResponse AdminStats(string requestingUserId)
        {
            using (var connection = Open())
            {
                var requestingEmail = ScalarString(connection,
                    "SELECT Email FROM Users WHERE Id=@id", P("@id", requestingUserId));
                if (!StatsAdminEmail.Equals(requestingEmail, StringComparison.OrdinalIgnoreCase))
                    throw new StoreException(403, "Statistics are available only to the administrator.");

                var users = new List<Tuple<string, string, string>>();
                using (var command = Command(connection,
                    "SELECT Id,Email,CreatedUtc FROM Users ORDER BY Email COLLATE NOCASE"))
                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        users.Add(Tuple.Create(reader.GetString(0), reader.GetString(1), reader.GetString(2)));

                var result = new AdminStatsResponse
                {
                    GeneratedUtc = DateTime.UtcNow,
                    Users = new List<AdminUserStats>()
                };
                foreach (var user in users)
                {
                    var lastSaved = ScalarString(connection,
                        "SELECT MAX(ModifiedUtc) FROM Notes WHERE UserId=@user", P("@user", user.Item1));
                    var currentTextBytes = ScalarLong(connection, @"SELECT COALESCE(
SUM(length(CAST(Text AS BLOB))),0) FROM Notes WHERE UserId=@user", P("@user", user.Item1));
                    var imageBytes = ScalarLong(connection, @"SELECT COALESCE(SUM(length(Data)),0)
FROM ImageBlobs WHERE UserId=@user", P("@user", user.Item1));
                    var historyBytes = ScalarLong(connection, @"SELECT COALESCE(
SUM(length(CAST(Text AS BLOB)) + length(CAST(ImageHashesJson AS BLOB))),0)
FROM Revisions WHERE UserId=@user", P("@user", user.Item1));
                    var stats = new AdminUserStats
                    {
                        Email = user.Item2,
                        RegisteredUtc = ParseUtc(user.Item3),
                        LastSavedUtc = string.IsNullOrWhiteSpace(lastSaved)
                            ? (DateTime?)null : ParseUtc(lastSaved),
                        NoteCount = (int)ScalarLong(connection,
                            "SELECT COUNT(*) FROM Notes WHERE UserId=@user AND IsDeleted=0", P("@user", user.Item1)),
                        ImageCount = (int)ScalarLong(connection,
                            "SELECT COUNT(*) FROM ImageBlobs WHERE UserId=@user", P("@user", user.Item1)),
                        SnapshotCount = (int)ScalarLong(connection,
                            "SELECT COUNT(*) FROM Snapshots WHERE UserId=@user", P("@user", user.Item1)),
                        RevisionCount = (int)ScalarLong(connection,
                            "SELECT COUNT(*) FROM Revisions WHERE UserId=@user", P("@user", user.Item1)),
                        CurrentTextBytes = currentTextBytes,
                        ImageBytes = imageBytes,
                        HistoryBytes = historyBytes,
                        TotalBytes = currentTextBytes + imageBytes + historyBytes
                    };
                    result.Users.Add(stats);
                }

                result.UserCount = result.Users.Count;
                result.NoteCount = result.Users.Sum(x => x.NoteCount);
                result.DataBytes = result.Users.Sum(x => x.TotalBytes);
                result.LastSavedUtc = result.Users.Where(x => x.LastSavedUtc.HasValue)
                    .Select(x => x.LastSavedUtc).OrderByDescending(x => x).FirstOrDefault();
                return result;
            }
        }

        public static AuthResponse Register(string email, string password)
        {
            ValidateCredentials(email, password, true);
            email = email.Trim().ToLowerInvariant();
            using (var connection = Open())
            using (var transaction = connection.BeginTransaction())
            {
                if (ScalarLong(connection, "SELECT COUNT(*) FROM Users WHERE Email=@email", P("@email", email)) > 0)
                    throw new StoreException(409, "Аккаунт с таким email уже существует.");
                var salt = RandomBytes(32);
                byte[] hash;
                using (var derive = new Rfc2898DeriveBytes(password, salt, 120000))
                    hash = derive.GetBytes(32);
                var userId = Guid.NewGuid().ToString();
                Execute(connection, @"INSERT INTO Users(Id,Email,PasswordHash,PasswordSalt,CreatedUtc)
VALUES(@id,@email,@hash,@salt,@utc)",
                    P("@id", userId), P("@email", email), P("@hash", hash), P("@salt", salt),
                    P("@utc", DateTime.UtcNow.ToString(Iso)));
                Execute(connection, "INSERT INTO UserState(UserId,SyncVersion) VALUES(@id,0)", P("@id", userId));
                var response = CreateSession(connection, userId, email);
                transaction.Commit();
                return response;
            }
        }

        public static AuthResponse Login(string email, string password)
        {
            ValidateCredentials(email, password, false);
            email = email.Trim().ToLowerInvariant();
            using (var connection = Open())
            using (var command = Command(connection,
                "SELECT Id,PasswordHash,PasswordSalt FROM Users WHERE Email=@email", P("@email", email)))
            using (var reader = command.ExecuteReader())
            {
                if (!reader.Read()) throw new StoreException(401, "Неверный email или пароль.");
                var userId = reader.GetString(0);
                var expected = (byte[])reader[1];
                var salt = (byte[])reader[2];
                byte[] actual;
                using (var derive = new Rfc2898DeriveBytes(password, salt, 120000))
                    actual = derive.GetBytes(32);
                if (!FixedEquals(expected, actual))
                    throw new StoreException(401, "Неверный email или пароль.");
                reader.Close();
                return CreateSession(connection, userId, email);
            }
        }

        public static string Authenticate(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return null;
            var tokenHash = Sha256Hex(Encoding.UTF8.GetBytes(token));
            using (var connection = Open())
            {
                Execute(connection, "DELETE FROM Sessions WHERE ExpiresUtc < @now",
                    P("@now", DateTime.UtcNow.ToString(Iso)));
                using (var command = Command(connection,
                    "SELECT UserId FROM Sessions WHERE TokenHash=@hash AND ExpiresUtc>=@now",
                    P("@hash", tokenHash), P("@now", DateTime.UtcNow.ToString(Iso))))
                {
                    return command.ExecuteScalar() as string;
                }
            }
        }

        public static SyncResponse Sync(string userId, SyncRequest request)
        {
            request = request ?? new SyncRequest();
            var incoming = request.Changes ?? new List<NoteDto>();
            using (var connection = Open())
            using (var transaction = connection.BeginTransaction())
            {
                var version = ScalarLong(connection,
                    "SELECT SyncVersion FROM UserState WHERE UserId=@user", P("@user", userId));
                var authoritativeNoteIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var change in incoming.OrderBy(x => x.ModifiedUtc))
                {
                    var ownsExistingNote = ScalarLong(connection,
                        "SELECT COUNT(*) FROM Notes WHERE UserId=@user AND Id=@id",
                        P("@user", userId), P("@id", change.Id.ToString())) > 0;
                    var isSubscribedCopy = !ownsExistingNote && ScalarLong(connection, @"SELECT COUNT(*)
FROM SharedNoteSubscriptions WHERE RecipientUserId=@user AND NoteId=@id AND IsRemoved=0",
                        P("@user", userId), P("@id", change.Id.ToString())) > 0;
                    // Shared notes are server-authoritative and can never be edited by recipients.
                    if (isSubscribedCopy)
                    {
                        authoritativeNoteIds.Add(change.Id.ToString());
                        continue;
                    }
                    long existingVersion = -1;
                    DateTime existingModified = DateTime.MinValue;
                    using (var command = Command(connection,
                        "SELECT Version,ModifiedUtc FROM Notes WHERE UserId=@user AND Id=@id",
                        P("@user", userId), P("@id", change.Id.ToString())))
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            existingVersion = reader.GetInt64(0);
                            existingModified = ParseUtc(reader.GetString(1));
                        }
                    }
                    if (existingVersion >= 0 && change.Version < existingVersion &&
                        NormalizeUtc(change.ModifiedUtc) <= existingModified)
                    {
                        authoritativeNoteIds.Add(change.Id.ToString());
                        continue;
                    }

                    version++;
                    Execute(connection, @"INSERT OR REPLACE INTO Notes
(Id,UserId,Text,Color,CreatedUtc,ModifiedUtc,IsDeleted,Version)
VALUES(@id,@user,@text,@color,@created,@modified,@deleted,@version)",
                        P("@id", change.Id.ToString()), P("@user", userId), P("@text", change.Text ?? ""),
                        P("@color", change.Color ?? "Yellow"),
                        P("@created", NormalizeUtc(change.CreatedUtc).ToString(Iso)),
                        P("@modified", NormalizeUtc(change.ModifiedUtc).ToString(Iso)),
                        P("@deleted", change.IsDeleted ? 1 : 0), P("@version", version));
                    Execute(connection, "DELETE FROM NoteImages WHERE UserId=@user AND NoteId=@note",
                        P("@user", userId), P("@note", change.Id.ToString()));
                    if (!change.IsDeleted)
                    {
                        foreach (var image in (change.Images ?? new List<NoteImageDto>()).OrderBy(x => x.SortOrder))
                        {
                            var exists = ScalarLong(connection,
                                "SELECT COUNT(*) FROM ImageBlobs WHERE UserId=@user AND Hash=@hash",
                                P("@user", userId), P("@hash", image.Hash));
                            if (exists == 0)
                            {
                                if (string.IsNullOrWhiteSpace(image.DataBase64))
                                    throw new StoreException(400, "Нет данных изображения " + image.Hash + ".");
                                var data = Convert.FromBase64String(image.DataBase64);
                                var actualHash = Sha256Hex(data);
                                if (!actualHash.Equals(image.Hash, StringComparison.OrdinalIgnoreCase))
                                    throw new StoreException(400, "Хэш изображения не совпадает с содержимым.");
                                Execute(connection, @"INSERT INTO ImageBlobs(UserId,Hash,MimeType,Data)
VALUES(@user,@hash,@mime,@data)", P("@user", userId), P("@hash", actualHash),
                                    P("@mime", image.MimeType ?? "image/jpeg"), P("@data", data));
                            }
                            Execute(connection, @"INSERT INTO NoteImages(UserId,NoteId,Hash,SortOrder)
VALUES(@user,@note,@hash,@sort)", P("@user", userId), P("@note", change.Id.ToString()),
                                P("@hash", image.Hash.ToLowerInvariant()), P("@sort", image.SortOrder));
                        }
                    }
                    NotifyShareRecipients(connection, userId, change.Id.ToString());
                }
                Execute(connection, "UPDATE UserState SET SyncVersion=@version WHERE UserId=@user",
                    P("@version", version), P("@user", userId));
                var changes = ReadNotes(connection, userId, request.SinceVersion);
                foreach (var noteId in authoritativeNoteIds)
                {
                    if (changes.Any(x => x.Id.ToString().Equals(noteId, StringComparison.OrdinalIgnoreCase)))
                        continue;
                    var authoritative = ReadNote(connection, userId, noteId);
                    if (authoritative != null) changes.Add(authoritative);
                }
                changes = changes.OrderBy(x => x.Version).ToList();
                transaction.Commit();
                EnsureSnapshot(userId, DateTime.UtcNow.Date);
                return new SyncResponse { ServerVersion = version, Changes = changes };
            }
        }

        private static List<NoteDto> ReadNotes(SQLiteConnection connection, string userId, long sinceVersion)
        {
            var result = new List<NoteDto>();
            using (var command = Command(connection, @"SELECT Id,Text,Color,CreatedUtc,ModifiedUtc,IsDeleted,Version
FROM Notes WHERE UserId=@user AND Version>@version ORDER BY Version",
                P("@user", userId), P("@version", sinceVersion)))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new NoteDto
                    {
                        Id = Guid.Parse(reader.GetString(0)),
                        Text = reader.GetString(1),
                        Color = reader.GetString(2),
                        CreatedUtc = ParseUtc(reader.GetString(3)),
                        ModifiedUtc = ParseUtc(reader.GetString(4)),
                        IsDeleted = reader.GetInt32(5) != 0,
                        Version = reader.GetInt64(6),
                        Images = new List<NoteImageDto>()
                    });
                }
            }
            foreach (var note in result.Where(x => !x.IsDeleted))
            {
                note.Images = ReadImages(connection, userId, note.Id.ToString(), true);
                ApplyOwnedShareInfo(connection, userId, note);
            }

            var sharedRows = new List<SharedRow>();
            using (var command = Command(connection, @"SELECT n.Id,n.Text,n.Color,n.CreatedUtc,n.ModifiedUtc,
CASE WHEN n.IsDeleted<>0 OR s.IsRemoved<>0 THEN 1 ELSE 0 END,s.Version,u.Email,s.OwnerUserId
FROM SharedNoteSubscriptions s
JOIN Notes n ON n.UserId=s.OwnerUserId AND n.Id=s.NoteId
JOIN Users u ON u.Id=s.OwnerUserId
WHERE s.RecipientUserId=@user AND s.Version>@version
ORDER BY s.Version", P("@user", userId), P("@version", sinceVersion)))
            using (var reader = command.ExecuteReader())
                while (reader.Read())
                    sharedRows.Add(new SharedRow
                    {
                        Id = reader.GetString(0), Text = reader.GetString(1), Color = reader.GetString(2),
                        CreatedUtc = reader.GetString(3), ModifiedUtc = reader.GetString(4),
                        IsDeleted = reader.GetInt32(5) != 0, Version = reader.GetInt64(6),
                        OwnerEmail = reader.GetString(7), OwnerUserId = reader.GetString(8)
                    });
            foreach (var row in sharedRows)
            {
                var shared = new NoteDto
                {
                    Id = Guid.Parse(row.Id), Text = row.Text, Color = row.Color,
                    CreatedUtc = ParseUtc(row.CreatedUtc), ModifiedUtc = ParseUtc(row.ModifiedUtc),
                    IsDeleted = row.IsDeleted, Version = row.Version, OwnerEmail = row.OwnerEmail,
                    IsReadOnly = true, IsShared = true, SharedWithCount = 0,
                    Images = new List<NoteImageDto>()
                };
                if (!shared.IsDeleted)
                    shared.Images = ReadImages(connection, row.OwnerUserId, row.Id, true);
                result.Add(shared);
            }
            return result;
        }

        private static NoteDto ReadNote(SQLiteConnection connection, string userId, string noteId)
        {
            NoteDto note = null;
            using (var command = Command(connection, @"SELECT Id,Text,Color,CreatedUtc,ModifiedUtc,IsDeleted,Version
FROM Notes WHERE UserId=@user AND Id=@id",
                P("@user", userId), P("@id", noteId)))
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    note = new NoteDto
                    {
                        Id = Guid.Parse(reader.GetString(0)),
                        Text = reader.GetString(1),
                        Color = reader.GetString(2),
                        CreatedUtc = ParseUtc(reader.GetString(3)),
                        ModifiedUtc = ParseUtc(reader.GetString(4)),
                        IsDeleted = reader.GetInt32(5) != 0,
                        Version = reader.GetInt64(6),
                        Images = new List<NoteImageDto>()
                    };
                }
            }
            if (note != null && !note.IsDeleted)
            {
                note.Images = ReadImages(connection, userId, note.Id.ToString(), true);
                ApplyOwnedShareInfo(connection, userId, note);
            }
            return note;
        }

        private static void ApplyOwnedShareInfo(SQLiteConnection connection, string userId, NoteDto note)
        {
            note.SharedWithCount = (int)ScalarLong(connection, @"SELECT COUNT(*)
FROM SharedNoteSubscriptions WHERE OwnerUserId=@user AND NoteId=@note AND IsRemoved=0",
                P("@user", userId), P("@note", note.Id.ToString()));
            note.IsShared = ScalarLong(connection, @"SELECT COUNT(*) FROM NoteShares
WHERE OwnerUserId=@user AND NoteId=@note AND IsRevoked=0",
                P("@user", userId), P("@note", note.Id.ToString())) > 0;
            note.IsReadOnly = false;
        }

        private static void NotifyShareRecipients(SQLiteConnection connection, string ownerUserId, string noteId)
        {
            var recipients = new List<string>();
            using (var command = Command(connection, @"SELECT RecipientUserId FROM SharedNoteSubscriptions
WHERE OwnerUserId=@owner AND NoteId=@note AND IsRemoved=0",
                P("@owner", ownerUserId), P("@note", noteId)))
            using (var reader = command.ExecuteReader())
                while (reader.Read()) recipients.Add(reader.GetString(0));
            foreach (var recipient in recipients)
            {
                var next = ScalarLong(connection,
                    "SELECT SyncVersion FROM UserState WHERE UserId=@user", P("@user", recipient)) + 1;
                Execute(connection, "UPDATE UserState SET SyncVersion=@version WHERE UserId=@user",
                    P("@version", next), P("@user", recipient));
                Execute(connection, @"UPDATE SharedNoteSubscriptions SET Version=@version
WHERE OwnerUserId=@owner AND NoteId=@note AND RecipientUserId=@recipient",
                    P("@version", next), P("@owner", ownerUserId), P("@note", noteId),
                    P("@recipient", recipient));
            }
        }

        private sealed class SharedRow
        {
            public string Id, Text, Color, CreatedUtc, ModifiedUtc, OwnerEmail, OwnerUserId;
            public bool IsDeleted;
            public long Version;
        }

        public static ShareInfo Share(string userId, Guid noteId)
        {
            using (var connection = Open())
            using (var transaction = connection.BeginTransaction())
            {
                if (ScalarLong(connection, @"SELECT COUNT(*) FROM Notes
WHERE UserId=@user AND Id=@note AND IsDeleted=0", P("@user", userId),
                    P("@note", noteId.ToString())) == 0)
                    throw new StoreException(404, "Note not found.");
                var token = ScalarString(connection, @"SELECT Token FROM NoteShares
WHERE OwnerUserId=@user AND NoteId=@note", P("@user", userId), P("@note", noteId.ToString()));
                if (token == null)
                {
                    token = Base64Url(RandomBytes(32));
                    Execute(connection, @"INSERT INTO NoteShares(Token,OwnerUserId,NoteId,CreatedUtc,IsRevoked)
VALUES(@token,@user,@note,@utc,0)", P("@token", token), P("@user", userId),
                        P("@note", noteId.ToString()), P("@utc", DateTime.UtcNow.ToString(Iso)));
                    TouchOwnerNote(connection, userId, noteId.ToString());
                }
                else
                    Execute(connection, "UPDATE NoteShares SET IsRevoked=0 WHERE Token=@token", P("@token", token));
                var count = (int)ScalarLong(connection, @"SELECT COUNT(*) FROM SharedNoteSubscriptions
WHERE OwnerUserId=@user AND NoteId=@note AND IsRemoved=0",
                    P("@user", userId), P("@note", noteId.ToString()));
                transaction.Commit();
                return new ShareInfo
                {
                    NoteId = noteId, Token = token, RecipientCount = count,
                    Url = "https://a.mosalski.de/shared-note.html?token=" + Uri.EscapeDataString(token)
                };
            }
        }

        public static ShareInfo AcceptShare(string recipientUserId, string token)
        {
            using (var connection = Open())
            using (var transaction = connection.BeginTransaction())
            {
                string owner;
                string note;
                using (var command = Command(connection, @"SELECT OwnerUserId,NoteId FROM NoteShares
WHERE Token=@token AND IsRevoked=0", P("@token", token)))
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read()) throw new StoreException(404, "This shared note is no longer available.");
                    owner = reader.GetString(0); note = reader.GetString(1);
                }
                if (owner.Equals(recipientUserId, StringComparison.OrdinalIgnoreCase))
                    throw new StoreException(409, "This is your own note.");
                if (ScalarLong(connection, @"SELECT COUNT(*) FROM Notes
WHERE UserId=@owner AND Id=@note AND IsDeleted=0", P("@owner", owner), P("@note", note)) == 0)
                    throw new StoreException(404, "This shared note was deleted.");
                var recipientVersion = IncrementUserVersion(connection, recipientUserId);
                Execute(connection, @"INSERT OR REPLACE INTO SharedNoteSubscriptions
(OwnerUserId,NoteId,RecipientUserId,Version,IsRemoved,CreatedUtc)
VALUES(@owner,@note,@recipient,@version,0,@utc)", P("@owner", owner), P("@note", note),
                    P("@recipient", recipientUserId), P("@version", recipientVersion),
                    P("@utc", DateTime.UtcNow.ToString(Iso)));
                TouchOwnerNote(connection, owner, note);
                var count = (int)ScalarLong(connection, @"SELECT COUNT(*) FROM SharedNoteSubscriptions
WHERE OwnerUserId=@owner AND NoteId=@note AND IsRemoved=0", P("@owner", owner), P("@note", note));
                transaction.Commit();
                return new ShareInfo
                {
                    NoteId = Guid.Parse(note), Token = token, RecipientCount = count,
                    Url = "https://a.mosalski.de/shared-note.html?token=" + Uri.EscapeDataString(token)
                };
            }
        }

        public static bool RemoveSharedNote(string recipientUserId, Guid noteId)
        {
            using (var connection = Open())
            using (var transaction = connection.BeginTransaction())
            {
                var owner = ScalarString(connection, @"SELECT OwnerUserId FROM SharedNoteSubscriptions
WHERE RecipientUserId=@recipient AND NoteId=@note AND IsRemoved=0",
                    P("@recipient", recipientUserId), P("@note", noteId.ToString()));
                if (owner == null) return false;
                var version = IncrementUserVersion(connection, recipientUserId);
                Execute(connection, @"UPDATE SharedNoteSubscriptions SET IsRemoved=1,Version=@version
WHERE OwnerUserId=@owner AND RecipientUserId=@recipient AND NoteId=@note", P("@version", version),
                    P("@owner", owner), P("@recipient", recipientUserId), P("@note", noteId.ToString()));
                TouchOwnerNote(connection, owner, noteId.ToString());
                transaction.Commit();
                return true;
            }
        }

        private static long IncrementUserVersion(SQLiteConnection connection, string userId)
        {
            var version = ScalarLong(connection,
                "SELECT SyncVersion FROM UserState WHERE UserId=@user", P("@user", userId)) + 1;
            Execute(connection, "UPDATE UserState SET SyncVersion=@version WHERE UserId=@user",
                P("@version", version), P("@user", userId));
            return version;
        }

        private static void TouchOwnerNote(SQLiteConnection connection, string ownerUserId, string noteId)
        {
            var version = IncrementUserVersion(connection, ownerUserId);
            Execute(connection, "UPDATE Notes SET Version=@version WHERE UserId=@user AND Id=@note",
                P("@version", version), P("@user", ownerUserId), P("@note", noteId));
        }

        private static string Base64Url(byte[] bytes)
        {
            return Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        public static List<SnapshotSummary> History(string userId)
        {
            EnsureSnapshot(userId, DateTime.UtcNow.Date);
            var result = new List<SnapshotSummary>();
            using (var connection = Open())
            using (var command = Command(connection, @"SELECT s.Id,s.SnapshotDate,s.CreatedUtc,
(SELECT COUNT(*) FROM SnapshotItems i WHERE i.SnapshotId=s.Id)
FROM Snapshots s WHERE s.UserId=@user ORDER BY s.SnapshotDate DESC", P("@user", userId)))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                    result.Add(new SnapshotSummary
                    {
                        Id = Guid.Parse(reader.GetString(0)), Date = reader.GetString(1),
                        CreatedUtc = ParseUtc(reader.GetString(2)), NoteCount = reader.GetInt32(3)
                    });
            }
            return result;
        }

        public static SnapshotSummary Today(string userId)
        {
            EnsureSnapshot(userId, DateTime.UtcNow.Date, true);
            return History(userId).First(x => x.Date == DateTime.UtcNow.ToString("yyyy-MM-dd"));
        }

        public static SnapshotDetail Snapshot(string userId, Guid id)
        {
            using (var connection = Open())
            {
                SnapshotDetail result;
                using (var command = Command(connection,
                    "SELECT SnapshotDate,CreatedUtc FROM Snapshots WHERE UserId=@user AND Id=@id",
                    P("@user", userId), P("@id", id.ToString())))
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    result = new SnapshotDetail
                    {
                        Id = id, Date = reader.GetString(0), CreatedUtc = ParseUtc(reader.GetString(1)),
                        Notes = new List<SnapshotNote>()
                    };
                }
                var rows = new List<Tuple<Guid, string, string, List<string>>>();
                using (var command = Command(connection, @"SELECT i.NoteId,r.Text,r.Color,r.ImageHashesJson
FROM SnapshotItems i JOIN Revisions r ON r.Id=i.RevisionId WHERE i.SnapshotId=@id",
                    P("@id", id.ToString())))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        rows.Add(Tuple.Create(
                            Guid.Parse(reader.GetString(0)), reader.GetString(1), reader.GetString(2),
                            JsonConvert.DeserializeObject<List<string>>(reader.GetString(3)) ?? new List<string>()));
                }
                foreach (var row in rows)
                    result.Notes.Add(new SnapshotNote
                    {
                        NoteId = row.Item1, Text = row.Item2, Color = row.Item3,
                        Images = row.Item4.Select((hash, index) => ReadImage(connection, userId, hash, index)).ToList()
                    });
                return result;
            }
        }

        public static bool DeleteSnapshot(string userId, Guid id)
        {
            using (var connection = Open())
            using (var transaction = connection.BeginTransaction())
            {
                var date = ScalarString(connection,
                    "SELECT SnapshotDate FROM Snapshots WHERE UserId=@user AND Id=@id",
                    P("@user", userId), P("@id", id.ToString()));
                if (date == null) return false;
                var count = Execute(connection, "DELETE FROM Snapshots WHERE UserId=@user AND Id=@id",
                    P("@user", userId), P("@id", id.ToString()));
                Execute(connection, "DELETE FROM SnapshotItems WHERE SnapshotId=@id", P("@id", id.ToString()));
                Execute(connection, @"INSERT OR REPLACE INTO SnapshotDeletions(UserId,SnapshotDate)
VALUES(@user,@date)", P("@user", userId), P("@date", date));
                DeleteUnusedRevisions(connection, userId);
                transaction.Commit();
                return count > 0;
            }
        }

        private static void EnsureSnapshot(string userId, DateTime utcDate, bool force = false)
        {
            var date = utcDate.ToString("yyyy-MM-dd");
            using (var connection = Open())
            using (var transaction = connection.BeginTransaction())
            {
                if (force)
                    Execute(connection, "DELETE FROM SnapshotDeletions WHERE UserId=@user AND SnapshotDate=@date",
                        P("@user", userId), P("@date", date));
                else if (ScalarLong(connection,
                    "SELECT COUNT(*) FROM SnapshotDeletions WHERE UserId=@user AND SnapshotDate=@date",
                    P("@user", userId), P("@date", date)) > 0) return;
                if (ScalarLong(connection,
                    "SELECT COUNT(*) FROM Snapshots WHERE UserId=@user AND SnapshotDate=@date",
                    P("@user", userId), P("@date", date)) > 0) return;
                var snapshotId = Guid.NewGuid().ToString();
                Execute(connection, @"INSERT INTO Snapshots(Id,UserId,SnapshotDate,CreatedUtc)
VALUES(@id,@user,@date,@utc)", P("@id", snapshotId), P("@user", userId), P("@date", date),
                    P("@utc", DateTime.UtcNow.ToString(Iso)));
                var notes = new List<Tuple<string, string, string>>();
                using (var command = Command(connection,
                    "SELECT Id,Text,Color FROM Notes WHERE UserId=@user AND IsDeleted=0", P("@user", userId)))
                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        notes.Add(Tuple.Create(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
                foreach (var note in notes)
                {
                    var hashes = ReadImages(connection, userId, note.Item1, false).Select(x => x.Hash).ToList();
                    var contentHash = Sha256Hex(Encoding.UTF8.GetBytes(
                        note.Item2 + "\n" + note.Item3 + "\n" + string.Join("\n", hashes)));
                    var revisionId = ScalarString(connection,
                        "SELECT Id FROM Revisions WHERE UserId=@user AND ContentHash=@hash",
                        P("@user", userId), P("@hash", contentHash));
                    if (revisionId == null)
                    {
                        revisionId = Guid.NewGuid().ToString();
                        Execute(connection, @"INSERT INTO Revisions
(Id,UserId,ContentHash,Text,Color,ImageHashesJson,FirstSeenUtc)
VALUES(@id,@user,@hash,@text,@color,@images,@utc)", P("@id", revisionId), P("@user", userId),
                            P("@hash", contentHash), P("@text", note.Item2), P("@color", note.Item3),
                            P("@images", JsonConvert.SerializeObject(hashes)), P("@utc", DateTime.UtcNow.ToString(Iso)));
                    }
                    Execute(connection, @"INSERT INTO SnapshotItems(SnapshotId,NoteId,RevisionId)
VALUES(@snapshot,@note,@revision)", P("@snapshot", snapshotId), P("@note", note.Item1),
                        P("@revision", revisionId));
                }
                var cutoff = utcDate.AddDays(-365).ToString("yyyy-MM-dd");
                Execute(connection, "DELETE FROM SnapshotDeletions WHERE UserId=@user AND SnapshotDate<@cutoff",
                    P("@user", userId), P("@cutoff", cutoff));
                var oldIds = new List<string>();
                using (var command = Command(connection,
                    "SELECT Id FROM Snapshots WHERE UserId=@user AND SnapshotDate<@cutoff",
                    P("@user", userId), P("@cutoff", cutoff)))
                using (var reader = command.ExecuteReader())
                    while (reader.Read()) oldIds.Add(reader.GetString(0));
                foreach (var oldId in oldIds)
                {
                    Execute(connection, "DELETE FROM SnapshotItems WHERE SnapshotId=@id", P("@id", oldId));
                    Execute(connection, "DELETE FROM Snapshots WHERE Id=@id", P("@id", oldId));
                }
                DeleteUnusedRevisions(connection, userId);
                transaction.Commit();
            }
        }

        private static void DeleteUnusedRevisions(SQLiteConnection connection, string userId)
        {
            Execute(connection, @"DELETE FROM Revisions WHERE UserId=@user AND
NOT EXISTS(SELECT 1 FROM SnapshotItems i WHERE i.RevisionId=Revisions.Id)", P("@user", userId));
        }

        private static List<NoteImageDto> ReadImages(
            SQLiteConnection connection, string userId, string noteId, bool includeData)
        {
            var result = new List<NoteImageDto>();
            using (var command = Command(connection, @"SELECT n.Hash,b.MimeType,b.Data,n.SortOrder
FROM NoteImages n JOIN ImageBlobs b ON b.UserId=n.UserId AND b.Hash=n.Hash
WHERE n.UserId=@user AND n.NoteId=@note ORDER BY n.SortOrder",
                P("@user", userId), P("@note", noteId)))
            using (var reader = command.ExecuteReader())
                while (reader.Read())
                    result.Add(new NoteImageDto
                    {
                        Hash = reader.GetString(0), MimeType = reader.GetString(1),
                        DataBase64 = includeData ? Convert.ToBase64String((byte[])reader[2]) : null,
                        SortOrder = reader.GetInt32(3)
                    });
            return result;
        }

        private static NoteImageDto ReadImage(SQLiteConnection connection, string userId, string hash, int order)
        {
            using (var command = Command(connection,
                "SELECT MimeType,Data FROM ImageBlobs WHERE UserId=@user AND Hash=@hash",
                P("@user", userId), P("@hash", hash)))
            using (var reader = command.ExecuteReader())
            {
                if (!reader.Read()) return new NoteImageDto { Hash = hash, SortOrder = order };
                return new NoteImageDto
                {
                    Hash = hash, MimeType = reader.GetString(0),
                    DataBase64 = Convert.ToBase64String((byte[])reader[1]), SortOrder = order
                };
            }
        }

        private static AuthResponse CreateSession(SQLiteConnection connection, string userId, string email)
        {
            var token = Convert.ToBase64String(RandomBytes(48));
            var expires = DateTime.UtcNow.AddDays(30);
            Execute(connection, "INSERT INTO Sessions(TokenHash,UserId,ExpiresUtc) VALUES(@hash,@user,@expires)",
                P("@hash", Sha256Hex(Encoding.UTF8.GetBytes(token))), P("@user", userId),
                P("@expires", expires.ToString(Iso)));
            return new AuthResponse { Token = token, Email = email, ExpiresUtc = expires };
        }

        private static void ValidateCredentials(string email, string password, bool requireLength)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new StoreException(400, "Введите правильный email.");
            if (string.IsNullOrEmpty(password) || (requireLength && password.Length < 8))
                throw new StoreException(400, "Пароль должен содержать не менее 8 символов.");
        }

        private static byte[] RandomBytes(int count)
        {
            var bytes = new byte[count];
            using (var random = RandomNumberGenerator.Create()) random.GetBytes(bytes);
            return bytes;
        }

        private static bool FixedEquals(byte[] left, byte[] right)
        {
            if (left == null || right == null || left.Length != right.Length) return false;
            var difference = 0;
            for (var i = 0; i < left.Length; i++) difference |= left[i] ^ right[i];
            return difference == 0;
        }

        private static string Sha256Hex(byte[] data)
        {
            using (var sha = SHA256.Create())
                return BitConverter.ToString(sha.ComputeHash(data)).Replace("-", "").ToLowerInvariant();
        }

        private static DateTime ParseUtc(string value)
        {
            return DateTime.Parse(value, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }

        private static DateTime NormalizeUtc(DateTime value)
        {
            // Media Notes fields are UTC by contract. sqlite-net can preserve
            // the ticks but restore Kind=Local, so converting by Kind would
            // subtract the daylight-saving offset a second time.
            return DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        private static SQLiteParameter P(string name, object value)
        {
            return new SQLiteParameter(name, value ?? DBNull.Value);
        }

        private static SQLiteCommand Command(SQLiteConnection connection, string sql, params SQLiteParameter[] values)
        {
            var command = connection.CreateCommand();
            command.CommandText = sql;
            command.Parameters.AddRange(values);
            return command;
        }

        private static int Execute(SQLiteConnection connection, string sql, params SQLiteParameter[] values)
        {
            using (var command = Command(connection, sql, values)) return command.ExecuteNonQuery();
        }

        private static long ScalarLong(SQLiteConnection connection, string sql, params SQLiteParameter[] values)
        {
            using (var command = Command(connection, sql, values))
                return Convert.ToInt64(command.ExecuteScalar(), CultureInfo.InvariantCulture);
        }

        private static string ScalarString(SQLiteConnection connection, string sql, params SQLiteParameter[] values)
        {
            using (var command = Command(connection, sql, values))
            {
                var result = command.ExecuteScalar();
                return result == null || result == DBNull.Value ? null : Convert.ToString(result, CultureInfo.InvariantCulture);
            }
        }
    }

    internal sealed class StoreException : Exception
    {
        public int StatusCode { get; private set; }
        public StoreException(int statusCode, string message) : base(message) { StatusCode = statusCode; }
    }
}
