namespace Uma.Eservices.VetumaConn
{
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Abstracts Vetuma strong authentication API in a mockable service
    /// </summary>
    public interface IStrongAuthenticationService
    {
        /// <summary>
        /// Authenticate user via Authenticattion service. Redirects user to service wher euser use 3rth party tools for Authentication
        /// </summary>
        /// <param name="language">TransactionLanguage 2-letter language code (EN,FI,SV)</param>
        /// <param name="uriModel">VetumaUriModel contains necessary uri links</param>
        /// <param name="transactionId">Unique TransactionId for authentication process</param>
        /// <param name="vetumaButtonText">Vetuma button text</param>
        /// <param name="vetumaButtonInstructions">Vetuma button instruction text</param>
        void Authenticate(TransactionLanguage language, VetumaUriModel uriModel, string transactionId, string vetumaButtonText, string vetumaButtonInstructions);

        /// <summary>
        /// Determine the result of the Vetuma authentication by examining the returned request (user is returned to our service)
        /// </summary>
        /// <param name="transactionId">Transaction id uniqu id to get payment action reference</param>
        /// <returns>VetumaAuthResponse object</returns>
        /// <remarks>Retrieves the current request internally from HttpContext</remarks>
        VetumaAuthResponse ProcessResult(string transactionId);
    }
}
