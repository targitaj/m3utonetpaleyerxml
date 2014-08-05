namespace Uma.Eservices.Web.Components
{
    /// <summary>
    /// Represents the result value of RECaptcha verification process.
    /// </summary>
    public enum RecaptchaVerificationResult
    {
        /// <summary>
        /// Verification failed but the exact reason is not known.
        /// </summary>
        UnknownError = 0,

        /// <summary>
        /// Verification succeeded.
        /// </summary>
        Success = 1,

        /// <summary>
        /// The user's response to RECaptcha challenge is incorrect.
        /// </summary>
        IncorrectCaptchaSolution = 2,

        /// <summary>
        /// The request parameters in the client-side cookie are invalid.
        /// </summary>
        InvalidCookieParameters = 3,

        /// <summary>
        /// The private supplied at the time of verification process is invalid.
        /// </summary>
        InvalidPrivateKey = 4,

        /// <summary>
        /// The user's response to the RECaptcha challenge is null or empty.
        /// </summary>
        NullOrEmptyCaptchaSolution = 5
    }
}
