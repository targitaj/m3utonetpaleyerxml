using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MediaNotes.LegacyApi.Tests
{
    public sealed class SharingTests
    {
        [Fact]
        public void Shared_note_is_read_only_updates_and_owner_deletion_propagates()
        {
            var suffix = Guid.NewGuid().ToString("N");
            var ownerAuth = NotesStore.Register("owner-" + suffix + "@example.test", "password123");
            var recipientAuth = NotesStore.Register("recipient-" + suffix + "@example.test", "password123");
            var owner = NotesStore.Authenticate(ownerAuth.Token);
            var recipient = NotesStore.Authenticate(recipientAuth.Token);
            var noteId = Guid.NewGuid();
            var created = DateTime.UtcNow.AddSeconds(-2);
            var first = NotesStore.Sync(owner, new SyncRequest
            {
                SinceVersion = 0,
                Changes = new List<NoteDto>
                {
                    Note(noteId, "first", created, created, false, 0)
                }
            });

            var link = NotesStore.Share(owner, noteId);
            Assert.StartsWith("https://a.mosalski.de/shared-note.html?token=", link.Url);
            NotesStore.AcceptShare(recipient, link.Token);
            var received = NotesStore.Sync(recipient, new SyncRequest { SinceVersion = 0 });
            var shared = Assert.Single(received.Changes.Where(x => x.Id == noteId));
            Assert.True(shared.IsReadOnly);
            Assert.True(shared.IsShared);
            Assert.Equal(ownerAuth.Email, shared.OwnerEmail);
            Assert.Equal("first", shared.Text);

            var modified = DateTime.UtcNow;
            NotesStore.Sync(owner, new SyncRequest
            {
                SinceVersion = first.ServerVersion,
                Changes = new List<NoteDto>
                {
                    Note(noteId, "second", created, modified, false, first.ServerVersion)
                }
            });
            var updated = NotesStore.Sync(recipient, new SyncRequest { SinceVersion = received.ServerVersion });
            Assert.Equal("second", Assert.Single(updated.Changes.Where(x => x.Id == noteId)).Text);

            NotesStore.Sync(owner, new SyncRequest
            {
                SinceVersion = first.ServerVersion,
                Changes = new List<NoteDto>
                {
                    Note(noteId, "second", created, modified.AddSeconds(1), true, 0)
                }
            });
            var deleted = NotesStore.Sync(recipient, new SyncRequest { SinceVersion = updated.ServerVersion });
            Assert.True(Assert.Single(deleted.Changes.Where(x => x.Id == noteId)).IsDeleted);
        }

        [Fact]
        public void Recipient_can_remove_only_own_subscription()
        {
            var suffix = Guid.NewGuid().ToString("N");
            var ownerAuth = NotesStore.Register("owner-" + suffix + "@example.test", "password123");
            var recipientAuth = NotesStore.Register("recipient-" + suffix + "@example.test", "password123");
            var owner = NotesStore.Authenticate(ownerAuth.Token);
            var recipient = NotesStore.Authenticate(recipientAuth.Token);
            var noteId = Guid.NewGuid();
            var now = DateTime.UtcNow;
            NotesStore.Sync(owner, new SyncRequest { SinceVersion = 0,
                Changes = new List<NoteDto> { Note(noteId, "kept", now, now, false, 0) } });
            var share = NotesStore.Share(owner, noteId);
            NotesStore.AcceptShare(recipient, share.Token);
            var before = NotesStore.Sync(recipient, new SyncRequest { SinceVersion = 0 });
            Assert.True(NotesStore.RemoveSharedNote(recipient, noteId));
            var after = NotesStore.Sync(recipient, new SyncRequest { SinceVersion = before.ServerVersion });
            Assert.True(Assert.Single(after.Changes.Where(x => x.Id == noteId)).IsDeleted);
            Assert.Equal("kept", Assert.Single(NotesStore.Sync(owner,
                new SyncRequest { SinceVersion = 0 }).Changes.Where(x => x.Id == noteId)).Text);
        }

        private static NoteDto Note(Guid id, string text, DateTime created, DateTime modified,
            bool deleted, long version) => new NoteDto
        {
            Id = id, Text = text, Color = "Yellow", CreatedUtc = created,
            ModifiedUtc = modified, IsDeleted = deleted, Version = version,
            Images = new List<NoteImageDto>()
        };
    }
}
