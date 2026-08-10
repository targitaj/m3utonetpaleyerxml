using System;
using System.Net;
using System.Web;
using System.Web.Http;

namespace MediaNotes.LegacyApi
{
    internal static class ApiSupport
    {
        public static HttpResponseException Error(Exception exception)
        {
            var store = exception as StoreException;
            return new HttpResponseException(store == null
                ? HttpStatusCode.InternalServerError
                : (HttpStatusCode)store.StatusCode);
        }

        public static void Require(bool condition, HttpStatusCode status)
        {
            if (!condition) throw new HttpResponseException(status);
        }

        public static string UserId()
        {
            var header = HttpContext.Current.Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(header) ||
                !header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) return null;
            return NotesStore.Authenticate(header.Substring("Bearer ".Length).Trim());
        }
    }

    public sealed class AuthController : ApiController
    {
        public object Post(string id, AuthRequest request)
        {
            try
            {
                ApiSupport.Require(id != null, HttpStatusCode.BadRequest);
                if (id.Equals("register", StringComparison.OrdinalIgnoreCase))
                    return NotesStore.Register(request == null ? null : request.Email,
                        request == null ? null : request.Password);
                if (id.Equals("login", StringComparison.OrdinalIgnoreCase))
                    return NotesStore.Login(request == null ? null : request.Email,
                        request == null ? null : request.Password);
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            catch (HttpResponseException) { throw; }
            catch (Exception ex) { throw ApiSupport.Error(ex); }
        }
    }

    public sealed class SyncController : ApiController
    {
        public SyncResponse Post(SyncRequest request)
        {
            try
            {
                var userId = ApiSupport.UserId();
                ApiSupport.Require(userId != null, HttpStatusCode.Unauthorized);
                return NotesStore.Sync(userId, request);
            }
            catch (HttpResponseException) { throw; }
            catch (Exception ex) { throw ApiSupport.Error(ex); }
        }
    }

    public sealed class HistoryController : ApiController
    {
        public object Get()
        {
            try
            {
                var userId = ApiSupport.UserId();
                ApiSupport.Require(userId != null, HttpStatusCode.Unauthorized);
                return NotesStore.History(userId);
            }
            catch (HttpResponseException) { throw; }
            catch (Exception ex) { throw ApiSupport.Error(ex); }
        }

        public object Get(string id)
        {
            try
            {
                var userId = ApiSupport.UserId();
                ApiSupport.Require(userId != null, HttpStatusCode.Unauthorized);
                Guid snapshotId;
                ApiSupport.Require(Guid.TryParse(id, out snapshotId), HttpStatusCode.BadRequest);
                var snapshot = NotesStore.Snapshot(userId, snapshotId);
                ApiSupport.Require(snapshot != null, HttpStatusCode.NotFound);
                return snapshot;
            }
            catch (HttpResponseException) { throw; }
            catch (Exception ex) { throw ApiSupport.Error(ex); }
        }

        public object Post(string id)
        {
            try
            {
                var userId = ApiSupport.UserId();
                ApiSupport.Require(userId != null, HttpStatusCode.Unauthorized);
                ApiSupport.Require("today".Equals(id, StringComparison.OrdinalIgnoreCase), HttpStatusCode.NotFound);
                return NotesStore.Today(userId);
            }
            catch (HttpResponseException) { throw; }
            catch (Exception ex) { throw ApiSupport.Error(ex); }
        }

        public void Delete(string id)
        {
            try
            {
                var userId = ApiSupport.UserId();
                ApiSupport.Require(userId != null, HttpStatusCode.Unauthorized);
                Guid snapshotId;
                ApiSupport.Require(Guid.TryParse(id, out snapshotId), HttpStatusCode.BadRequest);
                ApiSupport.Require(NotesStore.DeleteSnapshot(userId, snapshotId), HttpStatusCode.NotFound);
            }
            catch (HttpResponseException) { throw; }
            catch (Exception ex) { throw ApiSupport.Error(ex); }
        }
    }

    public sealed class ShareController : ApiController
    {
        public ShareInfo Post(string id)
        {
            try
            {
                var userId = ApiSupport.UserId();
                ApiSupport.Require(userId != null, HttpStatusCode.Unauthorized);
                ApiSupport.Require(!string.IsNullOrWhiteSpace(id), HttpStatusCode.BadRequest);
                Guid noteId;
                return Guid.TryParse(id, out noteId)
                    ? NotesStore.Share(userId, noteId)
                    : NotesStore.AcceptShare(userId, id);
            }
            catch (HttpResponseException) { throw; }
            catch (Exception ex) { throw ApiSupport.Error(ex); }
        }

        public void Delete(string id)
        {
            try
            {
                var userId = ApiSupport.UserId();
                ApiSupport.Require(userId != null, HttpStatusCode.Unauthorized);
                Guid noteId;
                ApiSupport.Require(Guid.TryParse(id, out noteId), HttpStatusCode.BadRequest);
                ApiSupport.Require(NotesStore.RemoveSharedNote(userId, noteId), HttpStatusCode.NotFound);
            }
            catch (HttpResponseException) { throw; }
            catch (Exception ex) { throw ApiSupport.Error(ex); }
        }
    }

    public sealed class HealthController : ApiController
    {
        public object Get()
        {
            return new { service = "MediaNotes", runtime = ".NET Framework", utc = DateTime.UtcNow };
        }
    }

    public sealed class StatsController : ApiController
    {
        public AdminStatsResponse Get()
        {
            try
            {
                var userId = ApiSupport.UserId();
                ApiSupport.Require(userId != null, HttpStatusCode.Unauthorized);
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    HttpContext.Current.Response.Cache.SetNoStore();
                }
                return NotesStore.AdminStats(userId);
            }
            catch (HttpResponseException) { throw; }
            catch (Exception ex) { throw ApiSupport.Error(ex); }
        }
    }
}
