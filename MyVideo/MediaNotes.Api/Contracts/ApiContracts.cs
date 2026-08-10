using System.ComponentModel.DataAnnotations;

namespace MediaNotes.Api.Contracts;

public sealed record RegisterRequest(
    [param: Required, EmailAddress] string Email,
    [param: Required, MinLength(8)] string Password);
public sealed record LoginRequest(
    [param: Required, EmailAddress] string Email,
    [param: Required] string Password);
public sealed record AuthResponse(string Token, string Email, DateTime ExpiresUtc);
public sealed record NoteImageDto(string Hash, string MimeType, string? DataBase64, int SortOrder);
public sealed record NoteDto(Guid Id, string Text, string Color, DateTime CreatedUtc,
    DateTime ModifiedUtc, bool IsDeleted, long Version, IReadOnlyList<NoteImageDto> Images);
public sealed record SyncRequest(long SinceVersion, IReadOnlyList<NoteDto> Changes);
public sealed record SyncResponse(long ServerVersion, IReadOnlyList<NoteDto> Changes);
public sealed record SnapshotSummary(Guid Id, DateOnly Date, DateTime CreatedUtc, int NoteCount);
public sealed record SnapshotNote(Guid NoteId, string Text, string Color, IReadOnlyList<NoteImageDto> Images);
public sealed record SnapshotDetail(Guid Id, DateOnly Date, DateTime CreatedUtc, IReadOnlyList<SnapshotNote> Notes);
