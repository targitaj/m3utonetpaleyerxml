namespace MediaNotes.Api.Models;

public sealed class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = "";
    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public long SyncVersion { get; set; }
}

public sealed class Note
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Text { get; set; } = "";
    public string Color { get; set; } = "Yellow";
    public DateTime CreatedUtc { get; set; }
    public DateTime ModifiedUtc { get; set; }
    public bool IsDeleted { get; set; }
    public long Version { get; set; }
    public List<NoteImage> Images { get; set; } = [];
}

public sealed class ImageBlob
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Hash { get; set; } = "";
    public string MimeType { get; set; } = "image/jpeg";
    public byte[] Data { get; set; } = [];
}

public sealed class NoteImage
{
    public Guid NoteId { get; set; }
    public Note Note { get; set; } = null!;
    public Guid ImageBlobId { get; set; }
    public ImageBlob ImageBlob { get; set; } = null!;
    public int SortOrder { get; set; }
}

public sealed class NoteRevision
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string ContentHash { get; set; } = "";
    public string Text { get; set; } = "";
    public string Color { get; set; } = "Yellow";
    public string ImageHashesJson { get; set; } = "[]";
    public DateTime FirstSeenUtc { get; set; } = DateTime.UtcNow;
}

public sealed class DailySnapshot
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public DateOnly SnapshotDate { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public List<SnapshotItem> Items { get; set; } = [];
}

public sealed class SnapshotItem
{
    public Guid DailySnapshotId { get; set; }
    public DailySnapshot Snapshot { get; set; } = null!;
    public Guid NoteId { get; set; }
    public Guid NoteRevisionId { get; set; }
    public NoteRevision Revision { get; set; } = null!;
}
