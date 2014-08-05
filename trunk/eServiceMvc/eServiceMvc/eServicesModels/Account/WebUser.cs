namespace Uma.Eservices.Models.Account
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Application authorized user object
    /// </summary>
    public class WebUser
    {
        /// <summary>
        /// Creates a new object of ApplicationUser with empty Logins, Claims and Roles collections
        /// </summary>
        public WebUser()
        {
            this.Logins = new List<WebUserLogin>();
            this.Claims = new List<WebUserClaim>();
            this.Roles = new List<WebRole>();
        }

        /// <summary>
        /// Application User Unique identifier in system
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the email of logged in user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Temporary storage field: Password hash of a user. 
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Temporary storage field: SecurityStamp value of a user state. 
        /// </summary>
        public string SecurityStamp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this user account is confirmed through e-mail/phone
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier from UMA system, if user is linked to one.
        /// </summary>
        public long? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the display name (user-friendly) to display on interface screens.
        /// </summary>
        public string DisplayName 
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            } 
        }

        /// <summary>
        /// Last name of a person/user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// First name of a person/user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Person identification code (of state) = HETU
        /// </summary>
        public string PersonCode { get; set; }

        /// <summary>
        /// Mobile phone number of a user - used to send SMS messages to user as notifications
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// User Birth Date
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Distinguishes Weak and Strong authenticated users.
        /// False (default) = weak (username/password)
        /// True - strong (BankID, Mobile cert. etc.)
        /// </summary>
        public bool IsStronglyAuthenticated { get; set; }

        /// <summary>
        /// Referential property to User External Logins collection (if any)
        /// </summary>
        public ICollection<WebUserLogin> Logins { get; set; }

        /// <summary>
        /// Referential property to User Claims (Type/Value) collection (if any)
        /// </summary>
        public ICollection<WebUserClaim> Claims { get; set; }

        /// <summary>
        /// Referential property to User Roles (he is member of) collection (if any)
        /// </summary>
        public ICollection<WebRole> Roles { get; set; }
    }
}