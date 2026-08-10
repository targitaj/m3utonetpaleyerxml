using System;
using System.Collections.Generic;

namespace MediaNotes.LegacyApi
{
    public sealed class AuthRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public sealed class AuthResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public DateTime ExpiresUtc { get; set; }
    }

    public sealed class NoteImageDto
    {
        public string Hash { get; set; }
        public string MimeType { get; set; }
        public string DataBase64 { get; set; }
        public int SortOrder { get; set; }
    }

    public sealed class NoteDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
        public bool IsDeleted { get; set; }
        public long Version { get; set; }
        public List<NoteImageDto> Images { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsShared { get; set; }
        public int SharedWithCount { get; set; }
        public string OwnerEmail { get; set; }
    }

    public sealed class ShareInfo
    {
        public Guid NoteId { get; set; }
        public string Token { get; set; }
        public string Url { get; set; }
        public int RecipientCount { get; set; }
    }

    public sealed class SyncRequest
    {
        public long SinceVersion { get; set; }
        public List<NoteDto> Changes { get; set; }
    }

    public sealed class SyncResponse
    {
        public long ServerVersion { get; set; }
        public List<NoteDto> Changes { get; set; }
    }

    public sealed class SnapshotSummary
    {
        public Guid Id { get; set; }
        public string Date { get; set; }
        public DateTime CreatedUtc { get; set; }
        public int NoteCount { get; set; }
    }

    public sealed class SnapshotNote
    {
        public Guid NoteId { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }
        public List<NoteImageDto> Images { get; set; }
    }

    public sealed class SnapshotDetail
    {
        public Guid Id { get; set; }
        public string Date { get; set; }
        public DateTime CreatedUtc { get; set; }
        public List<SnapshotNote> Notes { get; set; }
    }

    public sealed class AdminUserStats
    {
        public string Email { get; set; }
        public DateTime RegisteredUtc { get; set; }
        public DateTime? LastSavedUtc { get; set; }
        public int NoteCount { get; set; }
        public int ImageCount { get; set; }
        public int SnapshotCount { get; set; }
        public int RevisionCount { get; set; }
        public long CurrentTextBytes { get; set; }
        public long ImageBytes { get; set; }
        public long HistoryBytes { get; set; }
        public long TotalBytes { get; set; }
    }

    public sealed class AdminStatsResponse
    {
        public DateTime GeneratedUtc { get; set; }
        public int UserCount { get; set; }
        public int NoteCount { get; set; }
        public long DataBytes { get; set; }
        public DateTime? LastSavedUtc { get; set; }
        public List<AdminUserStats> Users { get; set; }
    }
}
