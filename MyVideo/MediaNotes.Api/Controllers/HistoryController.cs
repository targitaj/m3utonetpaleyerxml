using System.Text.Json;
using MediaNotes.Api.Contracts;
using MediaNotes.Api.Data;
using MediaNotes.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediaNotes.Api.Controllers;

[ApiController, Authorize]
[Route("api/history")]
public sealed class HistoryController(NotesDbContext db, SnapshotService snapshots) : ControllerBase
{
    [HttpGet]
    public async Task<IReadOnlyList<SnapshotSummary>> List(CancellationToken cancellationToken) =>
        await db.DailySnapshots.AsNoTracking().Where(x => x.UserId == User.UserId())
            .OrderByDescending(x => x.SnapshotDate)
            .Select(x => new SnapshotSummary(x.Id, x.SnapshotDate, x.CreatedUtc, x.Items.Count))
            .ToListAsync(cancellationToken);

    [HttpPost("today")]
    public async Task<ActionResult<SnapshotSummary>> Today(CancellationToken cancellationToken)
    {
        var snapshot = await snapshots.CreateAsync(
            User.UserId(), DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);
        return Ok(new SnapshotSummary(snapshot.Id, snapshot.SnapshotDate, snapshot.CreatedUtc, snapshot.Items.Count));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SnapshotDetail>> Get(Guid id, CancellationToken cancellationToken)
    {
        var userId = User.UserId();
        var snapshot = await db.DailySnapshots.AsNoTracking()
            .Include(x => x.Items).ThenInclude(x => x.Revision)
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);
        if (snapshot is null) return NotFound();

        var hashes = snapshot.Items.SelectMany(x =>
            JsonSerializer.Deserialize<string[]>(x.Revision.ImageHashesJson) ?? []).Distinct().ToList();
        var blobs = await db.ImageBlobs.AsNoTracking()
            .Where(x => x.UserId == userId && hashes.Contains(x.Hash))
            .ToDictionaryAsync(x => x.Hash, cancellationToken);
        var notes = snapshot.Items.Select(item =>
        {
            var imageHashes = JsonSerializer.Deserialize<string[]>(item.Revision.ImageHashesJson) ?? [];
            var images = imageHashes.Select((hash, index) =>
            {
                var blob = blobs[hash];
                return new NoteImageDto(hash, blob.MimeType, Convert.ToBase64String(blob.Data), index);
            }).ToList();
            return new SnapshotNote(item.NoteId, item.Revision.Text, item.Revision.Color, images);
        }).ToList();
        return Ok(new SnapshotDetail(snapshot.Id, snapshot.SnapshotDate, snapshot.CreatedUtc, notes));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var userId = User.UserId();
        var snapshot = await db.DailySnapshots.SingleOrDefaultAsync(
            x => x.UserId == userId && x.Id == id, cancellationToken);
        if (snapshot is null) return NotFound();
        db.DailySnapshots.Remove(snapshot);
        await db.SaveChangesAsync(cancellationToken);
        await snapshots.PruneAsync(userId, DateOnly.MinValue, cancellationToken);
        return NoContent();
    }
}
