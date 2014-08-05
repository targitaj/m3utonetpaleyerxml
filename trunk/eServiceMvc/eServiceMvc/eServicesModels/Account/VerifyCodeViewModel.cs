namespace Uma.Eservices.Models.Account
{
    /// <summary>
    /// Verify code for the user browser allowance view model
    /// </summary>
    public class VerifyCodeViewModel
    {
        /// <summary>
        /// Provider of Code send (SMS or EMAil)
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Verification code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// URL storage where to send user after authentication chain
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Should browser be enabled forever and ever
        /// </summary>
        public bool RememberBrowser { get; set; }
    }
}
