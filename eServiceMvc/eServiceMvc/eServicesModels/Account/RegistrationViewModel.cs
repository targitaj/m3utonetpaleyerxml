namespace Uma.Eservices.Models.Account
{
    /// <summary>
    /// View data model to handle user registraton operation
    /// </summary>
    public class RegistrationViewModel
    {
        /// <summary>
        /// Gets or sets the logging in user's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of logging in user.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password confirmation.
        /// </summary>
        public string PasswordConfirm { get; set; }
    }
}
