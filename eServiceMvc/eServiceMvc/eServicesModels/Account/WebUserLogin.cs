namespace Uma.Eservices.Models.Account
{
    /// <summary>
    /// Class description to hold User External Logins (external providers, like Facebook) login information
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Justification = "Screw you!")]
    public class WebUserLogin
    {
        /// <summary>
        /// Gets or sets the login provider name, like "Facebook".
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Justification = "Buggr off!")]
        public string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the provider unique identifier for user (key).
        /// </summary>
        public string ProviderKey { get; set; }

        /// <summary>
        /// Gets or sets the user's Display Name inside external provider store.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the email address of an user inside external provider store (used to register onto external provider).
        /// </summary>
        public string Email { get; set; }
    }
}
