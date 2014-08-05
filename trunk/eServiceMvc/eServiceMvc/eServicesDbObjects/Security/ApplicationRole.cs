namespace Uma.Eservices.DbObjects
{
    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// Persistence object for Application Role (for EF Identity)
    /// </summary>
    public class ApplicationRole : IdentityRole<int, ApplicationUserRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRole"/> class.
        /// </summary>
        public ApplicationRole()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRole"/> class.
        /// </summary>
        /// <param name="name">The name of a role.</param>
        public ApplicationRole(string name)
        {
            this.Name = name;
        }
    }
}
