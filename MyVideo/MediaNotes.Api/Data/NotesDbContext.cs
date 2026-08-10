using MediaNotes.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaNotes.Api.Data;

public sealed class NotesDbContext(DbContextOptions<NotesDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<ImageBlob> ImageBlobs => Set<ImageBlob>();
    public DbSet<NoteImage> NoteImages => Set<NoteImage>();
    public DbSet<NoteRevision> NoteRevisions => Set<NoteRevision>();
    public DbSet<DailySnapshot> DailySnapshots => Set<DailySnapshot>();
    public DbSet<SnapshotItem> SnapshotItems => Set<SnapshotItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Note>().HasIndex(x => new { x.UserId, x.Version });
        modelBuilder.Entity<ImageBlob>().HasIndex(x => new { x.UserId, x.Hash }).IsUnique();
        modelBuilder.Entity<NoteImage>().HasKey(x => new { x.NoteId, x.ImageBlobId });
        modelBuilder.Entity<NoteRevision>().HasIndex(x => new { x.UserId, x.ContentHash }).IsUnique();
        modelBuilder.Entity<DailySnapshot>().HasIndex(x => new { x.UserId, x.SnapshotDate }).IsUnique();
        modelBuilder.Entity<SnapshotItem>().HasKey(x => new { x.DailySnapshotId, x.NoteId });
        modelBuilder.Entity<Note>().HasMany(x => x.Images).WithOne(x => x.Note)
            .HasForeignKey(x => x.NoteId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<DailySnapshot>().HasMany(x => x.Items).WithOne(x => x.Snapshot)
            .HasForeignKey(x => x.DailySnapshotId).OnDelete(DeleteBehavior.Cascade);
    }
}
