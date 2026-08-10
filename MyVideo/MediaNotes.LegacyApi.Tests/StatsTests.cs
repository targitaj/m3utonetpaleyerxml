using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MediaNotes.LegacyApi.Tests
{
    public sealed class StatsTests
    {
        [Fact]
        public void Statistics_require_admin_and_report_saved_data()
        {
            var suffix = Guid.NewGuid().ToString("N");
            var regularAuth = NotesStore.Register("stats-" + suffix + "@example.test", "password123");
            var regularId = NotesStore.Authenticate(regularAuth.Token);
            var forbidden = Assert.Throws<StoreException>(() => NotesStore.AdminStats(regularId));
            Assert.Equal(403, forbidden.StatusCode);

            AuthResponse adminAuth;
            try { adminAuth = NotesStore.Register("mosala@gmail.com", "stats-password-123"); }
            catch (StoreException ex) when (ex.StatusCode == 409)
            {
                adminAuth = NotesStore.Login("mosala@gmail.com", "stats-password-123");
            }
            var adminId = NotesStore.Authenticate(adminAuth.Token);
            var now = DateTime.UtcNow;
            var marker = "statistics-" + suffix;
            NotesStore.Sync(adminId, new SyncRequest
            {
                SinceVersion = 0,
                Changes = new List<NoteDto>
                {
                    new NoteDto
                    {
                        Id = Guid.NewGuid(), Text = marker, Color = "Yellow",
                        CreatedUtc = now, ModifiedUtc = now, IsDeleted = false,
                        Images = new List<NoteImageDto>()
                    }
                }
            });

            var stats = NotesStore.AdminStats(adminId);
            var admin = Assert.Single(stats.Users.Where(x => x.Email == "mosala@gmail.com"));
            Assert.True(admin.NoteCount >= 1);
            Assert.True(admin.CurrentTextBytes >= marker.Length);
            Assert.True(admin.TotalBytes >= admin.CurrentTextBytes);
            Assert.NotNull(admin.LastSavedUtc);
            Assert.True(stats.UserCount >= 2);
        }
    }
}
