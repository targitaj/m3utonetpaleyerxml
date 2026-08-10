using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MediaNotes.LegacyApi.Tests
{
    public sealed class UtcTimestampTests
    {
        [Fact]
        public void Sync_does_not_shift_utc_clock_value_when_kind_is_local()
        {
            var suffix = Guid.NewGuid().ToString("N");
            var auth = NotesStore.Register("utc-" + suffix + "@example.test", "password123");
            var userId = NotesStore.Authenticate(auth.Token);
            var noteId = Guid.NewGuid();
            var sqliteValue = new DateTime(2026, 8, 7, 17, 45, 0, DateTimeKind.Local);

            NotesStore.Sync(userId, new SyncRequest
            {
                SinceVersion = 0,
                Changes = new List<NoteDto>
                {
                    new NoteDto
                    {
                        Id = noteId, Text = "utc test", Color = "Yellow",
                        CreatedUtc = sqliteValue, ModifiedUtc = sqliteValue,
                        Images = new List<NoteImageDto>()
                    }
                }
            });

            var stored = Assert.Single(NotesStore.Sync(userId,
                new SyncRequest { SinceVersion = 0 }).Changes.Where(x => x.Id == noteId));
            Assert.Equal(DateTimeKind.Utc, stored.ModifiedUtc.Kind);
            Assert.Equal(17, stored.ModifiedUtc.Hour);
            Assert.Equal(45, stored.ModifiedUtc.Minute);
        }
    }
}
