namespace Uma.Eservices.Logic.Features.Account
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Wrapper for Identity Role manager to manage User access (via Roles)
    /// </summary>
    public class ApplicationRoleManager : RoleManager<ApplicationRole, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRoleManager"/> class.
        /// </summary>
        /// <param name="roleStore">The role store with DB.</param>
        public ApplicationRoleManager(IRoleStore<ApplicationRole, int> roleStore) : base(roleStore)
        {
        }

        /// <summary>
        /// Creates Role manager for OWIn usage
        /// </summary>
        /// <param name="options">The options passed from OWIN.</param>
        /// <param name="context">The context of OWIN.</param>
        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<ApplicationRole, int, ApplicationUserRole>(context.Get<DatabaseContext>()));
        }
    }
}
