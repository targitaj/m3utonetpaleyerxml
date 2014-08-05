namespace Uma.Eservices.DbAccess
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Store object for EF Identity
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApplicationRoleStore : RoleStore<ApplicationRole, int, ApplicationUserRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRoleStore"/> class.
        /// </summary>
        /// <param name="context">The context of database.</param>
        public ApplicationRoleStore(DatabaseContext context) : base(context)
        {
        }
    }
}
