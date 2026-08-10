using System.Security.Cryptography;
using MediaNotes.Api.Contracts;
using MediaNotes.Api.Data;
using MediaNotes.Api.Models;
using MediaNotes.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediaNotes.Api.Controllers;

[ApiController, Authorize]
[Route("api/sync")]
public sealed class SyncController(NotesDbContext db) : ControllerBase
{
    [HttpPost]
    [RequestSizeLimit(25_000_000)]
    public async Task<ActionResult<SyncResponse>> Sync(SyncRequest request, CancellationToken cancellationToken)
    {
        var userId = User.UserId();
        var user = await db.Users.SingleAsync(x => x.Id == userId, cancellationToken);
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        var authoritativeNoteIds = new HashSet<Guid>();

        foreach (var incoming in request.Changes.OrderBy(x => x.ModifiedUtc))
        {
            var note = await db.Notes.Include(x => x.Images).ThenInclude(x => x.ImageBlob)
                .SingleOrDefaultAsync(x => x.UserId == userId && x.Id == incoming.Id, cancellationToken);
            if (note is not null && incoming.Version < note.Version && incoming.ModifiedUtc <= note.ModifiedUtc)
            {
                authoritativeNoteIds.Add(note.Id);
                continue;
            }

            if (note is null)
            {
                note = new Note { Id = incoming.Id, UserId = userId, CreatedUtc = incoming.CreatedUtc };
                db.Notes.Add(note);
            }

            note.Text = incoming.Text;
            note.Color = incoming.Color;
            note.ModifiedUtc = incoming.ModifiedUtc.ToUniversalTime();
            note.IsDeleted = incoming.IsDeleted;
            note.Version = ++user.SyncVersion;

            var incomingHashes = note.IsDeleted
                ? new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                : incoming.Images.Select(x => x.Hash).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var removedLinks = note.Images.Where(x => !incomingHashes.Contains(x.ImageBlob.Hash)).ToList();
            db.NoteImages.RemoveRange(removedLinks);
            foreach (var removed in removedLinks) note.Images.Remove(removed);

            if (!note.IsDeleted)
            {
                foreach (var image in incoming.Images.OrderBy(x => x.SortOrder))
                {
                    var existingLink = note.Images.SingleOrDefault(
                        x => x.ImageBlob.Hash.Equals(image.Hash, StringComparison.OrdinalIgnoreCase));
                    if (existingLink is not null)
                    {
                        existingLink.SortOrder = image.SortOrder;
                        continue;
                    }
                    var blob = await db.ImageBlobs.SingleOrDefaultAsync(
                        x => x.UserId == userId && x.Hash == image.Hash, cancellationToken);
                    if (blob is null)
                    {
                        if (string.IsNullOrWhiteSpace(image.DataBase64))
                            return BadRequest(new { message = $"Нет данных изображения {image.Hash}." });
                        var bytes = Convert.FromBase64String(image.DataBase64);
                        var actualHash = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
                        if (!actualHash.Equals(image.Hash, StringComparison.OrdinalIgnoreCase))
                            return BadRequest(new { message = "Хэш изображения не совпадает с содержимым." });
                        blob = new ImageBlob
                        {
                            UserId = userId, Hash = actualHash, MimeType = image.MimeType, Data = bytes
                        };
                        db.ImageBlobs.Add(blob);
                    }
                    note.Images.Add(new NoteImage
                    {
                        NoteId = note.Id, ImageBlob = blob, SortOrder = image.SortOrder
                    });
                }
            }
        }

        await db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        var changes = await db.Notes.AsNoTracking().Include(x => x.Images).ThenInclude(x => x.ImageBlob)
            .Where(x => x.UserId == userId &&
                        (x.Version > request.SinceVersion || authoritativeNoteIds.Contains(x.Id)))
            .OrderBy(x => x.Version).ToListAsync(cancellationToken);
        return Ok(new SyncResponse(user.SyncVersion, changes.Select(ToDto).ToList()));
    }

    private static NoteDto ToDto(Note note) => new(
        note.Id, note.Text, note.Color, note.CreatedUtc, note.ModifiedUtc, note.IsDeleted, note.Version,
        note.Images.OrderBy(x => x.SortOrder).Select(x => new NoteImageDto(
            x.ImageBlob.Hash, x.ImageBlob.MimeType, Convert.ToBase64String(x.ImageBlob.Data), x.SortOrder)).ToList());
}
