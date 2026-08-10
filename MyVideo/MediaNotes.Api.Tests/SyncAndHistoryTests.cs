using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using MediaNotes.Api.Contracts;
using MediaNotes.Api.Data;
using MediaNotes.Api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MediaNotes.Api.Tests;

public sealed class TestApplication : WebApplicationFactory<Program>
{
    public string DatabasePath { get; } =
        Path.Combine(Path.GetTempPath(), $"medianotes-test-{Guid.NewGuid():N}.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:MediaNotes"] = $"Data Source={DatabasePath}"
            });
        });
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<NotesDbContext>>();
            services.RemoveAll<NotesDbContext>();
            services.AddDbContext<NotesDbContext>(options => options.UseSqlite($"Data Source={DatabasePath}"));
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        SqliteConnection.ClearAllPools();
        if (File.Exists(DatabasePath)) File.Delete(DatabasePath);
    }
}

public sealed class SyncAndHistoryTests : IClassFixture<TestApplication>
{
    private readonly TestApplication _application;
    private readonly HttpClient _client;

    public SyncAndHistoryTests(TestApplication application)
    {
        _application = application;
        _client = application.CreateClient();
    }

    [Fact]
    public async Task Register_sync_and_deduplicated_snapshots_work_end_to_end()
    {
        var email = $"test-{Guid.NewGuid():N}@example.com";
        var authResponse = await _client.PostAsJsonAsync(
            "/api/auth/register", new RegisterRequest(email, "SecurePass123!"));
        authResponse.EnsureSuccessStatusCode();
        var auth = await authResponse.Content.ReadFromJsonAsync<AuthResponse>();
        Assert.NotNull(auth);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);

        var noteId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var imageBytes = "small-image-payload"u8.ToArray();
        var imageHash = Convert.ToHexString(SHA256.HashData(imageBytes)).ToLowerInvariant();
        var syncResponse = await _client.PostAsJsonAsync("/api/sync", new SyncRequest(0,
        [
            new NoteDto(noteId, "Тестовая заметка", "Yellow", now, now, false, 0,
            [
                new NoteImageDto(imageHash, "image/jpeg", Convert.ToBase64String(imageBytes), 0)
            ])
        ]));
        syncResponse.EnsureSuccessStatusCode();
        var sync = await syncResponse.Content.ReadFromJsonAsync<SyncResponse>();
        Assert.NotNull(sync);
        Assert.Equal(1, sync.ServerVersion);
        Assert.Equal("Тестовая заметка", Assert.Single(sync.Changes).Text);

        var updateTime = now.AddMinutes(1);
        var updateResponse = await _client.PostAsJsonAsync("/api/sync", new SyncRequest(1,
        [
            new NoteDto(noteId, "Обновлённая заметка", "Blue", now, updateTime, false, 1,
            [
                new NoteImageDto(imageHash, "image/jpeg", Convert.ToBase64String(imageBytes), 0)
            ])
        ]));
        updateResponse.EnsureSuccessStatusCode();
        var updated = await updateResponse.Content.ReadFromJsonAsync<SyncResponse>();
        Assert.NotNull(updated);
        Assert.Equal(2, updated.ServerVersion);

        var todayResponse = await _client.PostAsync("/api/history/today", null);
        todayResponse.EnsureSuccessStatusCode();
        var today = await todayResponse.Content.ReadFromJsonAsync<SnapshotSummary>();
        Assert.NotNull(today);
        var detail = await _client.GetFromJsonAsync<SnapshotDetail>($"/api/history/{today.Id}");
        Assert.NotNull(detail);
        var snapshotNote = Assert.Single(detail.Notes);
        Assert.Equal("Обновлённая заметка", snapshotNote.Text);
        Assert.Equal(imageHash, Assert.Single(snapshotNote.Images).Hash);

        using var scope = _application.Services.CreateScope();
        var snapshots = scope.ServiceProvider.GetRequiredService<SnapshotService>();
        var db = scope.ServiceProvider.GetRequiredService<NotesDbContext>();
        var userId = await db.Users.Where(x => x.Email == email).Select(x => x.Id).SingleAsync();
        await snapshots.CreateAsync(userId, DateOnly.FromDateTime(now).AddDays(1), CancellationToken.None);
        Assert.Equal(2, await db.DailySnapshots.CountAsync(x => x.UserId == userId));
        Assert.Equal(1, await db.NoteRevisions.CountAsync(x => x.UserId == userId));
    }

    [Fact]
    public async Task Rejected_stale_change_returns_authoritative_note_even_when_since_version_is_current()
    {
        var email = $"conflict-{Guid.NewGuid():N}@example.com";
        var authResponse = await _client.PostAsJsonAsync(
            "/api/auth/register", new RegisterRequest(email, "SecurePass123!"));
        authResponse.EnsureSuccessStatusCode();
        var auth = await authResponse.Content.ReadFromJsonAsync<AuthResponse>();
        Assert.NotNull(auth);
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", auth.Token);

        var noteId = Guid.NewGuid();
        var created = DateTime.UtcNow.AddMinutes(-5);
        var initial = new NoteDto(noteId, "initial", "Yellow", created, created,
            false, 0, []);
        var firstResponse = await _client.PostAsJsonAsync(
            "/api/sync", new SyncRequest(0, [initial]));
        firstResponse.EnsureSuccessStatusCode();

        var authoritativeTime = created.AddMinutes(2);
        var authoritative = initial with
        {
            Text = "authoritative",
            ModifiedUtc = authoritativeTime,
            Version = 1
        };
        var secondResponse = await _client.PostAsJsonAsync(
            "/api/sync", new SyncRequest(1, [authoritative]));
        secondResponse.EnsureSuccessStatusCode();
        var second = await secondResponse.Content.ReadFromJsonAsync<SyncResponse>();
        Assert.NotNull(second);
        Assert.Equal(2, second.ServerVersion);

        var stale = initial with
        {
            Text = "stale edit",
            ModifiedUtc = created.AddMinutes(1),
            Version = 1
        };
        var conflictResponse = await _client.PostAsJsonAsync(
            "/api/sync", new SyncRequest(2, [stale]));
        conflictResponse.EnsureSuccessStatusCode();
        var conflict = await conflictResponse.Content.ReadFromJsonAsync<SyncResponse>();
        Assert.NotNull(conflict);
        Assert.Equal(2, conflict.ServerVersion);
        var returned = Assert.Single(conflict.Changes);
        Assert.Equal(noteId, returned.Id);
        Assert.Equal("authoritative", returned.Text);
        Assert.Equal(2, returned.Version);
    }
}
