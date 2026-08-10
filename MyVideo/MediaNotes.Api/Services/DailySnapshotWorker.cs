using MediaNotes.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace MediaNotes.Api.Services;

public sealed class DailySnapshotWorker(IServiceScopeFactory scopeFactory, ILogger<DailySnapshotWorker> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<NotesDbContext>();
                var snapshots = scope.ServiceProvider.GetRequiredService<SnapshotService>();
                var userIds = await db.Users.Select(x => x.Id).ToListAsync(stoppingToken);
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                foreach (var userId in userIds)
                {
                    await snapshots.CreateAsync(userId, today, stoppingToken);
                    await snapshots.PruneAsync(userId, today.AddDays(-365), stoppingToken);
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogError(ex, "Daily MediaNotes snapshot failed.");
            }
            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }
}
