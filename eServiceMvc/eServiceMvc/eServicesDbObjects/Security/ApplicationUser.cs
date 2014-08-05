namespace Uma.Eservices.DbObjects
{
    using System;
    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// Application user persistence object for EF Identity
    /// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    /// </summary>
    public class ApplicationUser : IdentityUser<int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        /// <summary>
        /// UMA Customer ID, which is linked to this user
        /// </summary>
        public long? CustomerId { get; set; }

        /// <summary>
        /// First name of a user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of a user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Birthdate of a user
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Person Code (HETU) for user
        /// </summary>
        public string PersonCode { get; set; }

        /// <summary>
        /// This distinguishes Weak and Strong authenticated users.
        /// False (default) = Weak authentication (Username/Password)
        /// True - Strongly authenticated users (Bank IDs, Electronic cards, Mobile certificates)
        /// </summary>
        public bool IsStronglyAuthenticated { get; set; }
    }
}
