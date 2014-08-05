namespace Uma.Eservices.Models.Account
{
    /// <summary>
    /// View Model for Login form(s)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Justification = "Screw you!")]
    public class LoginViewModel
    {
        /// <summary>
        /// Gets or sets the logging in user's username.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of logging in user.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [remember me].
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// If return URL is passed via GET (MVC behavior) - this property should be populated with it and used in Post redirect
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "It travels through system as string")]
        public string ReturnUrl { get; set; }
    }
}
