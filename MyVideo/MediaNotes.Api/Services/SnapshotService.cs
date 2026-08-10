using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MediaNotes.Api.Data;
using MediaNotes.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaNotes.Api.Services;

public sealed class SnapshotService(NotesDbContext db)
{
    public async Task<DailySnapshot> CreateAsync(Guid userId, DateOnly date, CancellationToken cancellationToken)
    {
        var existing = await db.DailySnapshots.Include(x => x.Items)
            .SingleOrDefaultAsync(x => x.UserId == userId && x.SnapshotDate == date, cancellationToken);
        if (existing is not null) return existing;

        var notes = await db.Notes.AsNoTracking().Include(x => x.Images).ThenInclude(x => x.ImageBlob)
            .Where(x => x.UserId == userId && !x.IsDeleted).ToListAsync(cancellationToken);
        var snapshot = new DailySnapshot { UserId = userId, SnapshotDate = date };
        foreach (var note in notes)
        {
            var hashes = note.Images.OrderBy(x => x.SortOrder).Select(x => x.ImageBlob.Hash).ToArray();
            var hashInput = $"{note.Text}\n{note.Color}\n{string.Join('\n', hashes)}";
            var contentHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(hashInput))).ToLowerInvariant();
            var revision = await db.NoteRevisions.SingleOrDefaultAsync(
                x => x.UserId == userId && x.ContentHash == contentHash, cancellationToken);
            if (revision is null)
            {
                revision = new NoteRevision
                {
                    UserId = userId, ContentHash = contentHash, Text = note.Text, Color = note.Color,
                    ImageHashesJson = JsonSerializer.Serialize(hashes)
                };
                db.NoteRevisions.Add(revision);
            }
            snapshot.Items.Add(new SnapshotItem { NoteId = note.Id, Revision = revision });
        }
        db.DailySnapshots.Add(snapshot);
        await db.SaveChangesAsync(cancellationToken);
        return snapshot;
    }

    public async Task PruneAsync(Guid userId, DateOnly cutoff, CancellationToken cancellationToken)
    {
        var old = await db.DailySnapshots.Where(x => x.UserId == userId && x.SnapshotDate < cutoff)
            .ToListAsync(cancellationToken);
        db.DailySnapshots.RemoveRange(old);
        await db.SaveChangesAsync(cancellationToken);
        var usedRevisionIds = db.SnapshotItems.Select(x => x.NoteRevisionId);
        var unused = await db.NoteRevisions.Where(x => x.UserId == userId && !usedRevisionIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
        db.NoteRevisions.RemoveRange(unused);
        await db.SaveChangesAsync(cancellationToken);
    }
}
