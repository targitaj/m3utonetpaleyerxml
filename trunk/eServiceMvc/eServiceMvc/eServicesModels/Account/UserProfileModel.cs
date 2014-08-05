namespace Uma.Eservices.Models.Account
{
    using System;

    /// <summary>
    /// Model for User Profile screens
    /// </summary>
    public class UserProfileModel
    {
        /// <summary>
        /// Last name of a person/user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// First name of a person/user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets the display name = full name of a user.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        /// <summary>
        /// User birthday date
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Person state given Person Code (HETU)
        /// </summary>
        public string PersonCode { get; set; }

        /// <summary>
        /// Email address of a person/user - used in Account information record (username)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Mobile phone number of a user - used to send SMS messages to user as notifications
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Calculated read-onlu property to output user Full name (concatenation of FirstName and LastName
        /// </summary>
        public string FullName  
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        /// <summary>
        /// Formats DateTime for Date output in user readable format
        /// </summary>
        public string BirthdayDisplay {
            get
            {
                if (this.BirthDate.HasValue)
                {
                    return this.BirthDate.Value.ToLongDateString();
                }

                return string.Empty;
            }
        }
    }
}
