namespace Uma.Eservices.DbAccess
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Store object for EF Identity
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserStore"/> class.
        /// </summary>
        /// <param name="context">The context of database.</param>
        public ApplicationUserStore(DatabaseContext context) : base(context)
        {
        }
    }
}
