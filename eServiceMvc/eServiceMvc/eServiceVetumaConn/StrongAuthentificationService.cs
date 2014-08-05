namespace Uma.Eservices.VetumaConn
{
    using System;
    using System.Collections.ObjectModel;
    using Fujitsu.Vetuma.Toolkit;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Abstracts Vetuma strong authentication API in a mockable service
    /// </summary>
    public class StrongAuthenticationService : IStrongAuthenticationService
    {
        /// <summary>
        /// Vetuma service object
        /// </summary>
        private IVetumaService vetumeService;

        /// <summary>
        /// Vetuma utilities object
        /// </summary>
        private IVetumaUtilities vetumaUtilities;

        /// <summary>
        /// Class contructor
        /// </summary>
        /// <param name="vetumaService">IVetumaService object</param>
        /// <param name="vetumaUtilities">IVetumaUtilities object</param>
        public StrongAuthenticationService(IVetumaService vetumaService, IVetumaUtilities vetumaUtilities)
        {
            this.vetumeService = vetumaService;
            this.vetumaUtilities = vetumaUtilities;
        }

        /// <summary>
        /// Authenticate user via Authenticattion service. Redirects user to service wher euser use 3rth party tools for Authentication
        /// Method uses strings, keys from config file.
        /// </summary>
        /// <param name="language">TransactionLanguage 2-letter language code (EN,FI,SV)</param>
        /// <param name="uriModel">VetumaUriModel contains necessary uri links</param>
        /// <param name="transactionId">Unique TransactionId for authentication process</param>
        /// <param name="vetumaButtonText">Vetuma button text</param>
        /// <param name="vetumaButtonInstructions">Vetuma button instruction text</param>
        public void Authenticate(TransactionLanguage language, VetumaUriModel uriModel, string transactionId, string vetumaButtonText, string vetumaButtonInstructions)
        {
            // Allow authentication through banks and with an HST card
            Collection<VetumaLoginMethod> methods = new Collection<VetumaLoginMethod>();
            methods.Add(VetumaLoginMethod.Tupas);
            methods.Add(VetumaLoginMethod.HST);

            // first two parameters are related to the VetumaRequest rendering a button to facilitate 
            //    // transferring to Vetuma when the user has javascript disabled.
            VetumaAuthenticationRequest request = new VetumaAuthenticationRequest(vetumaButtonText, vetumaButtonInstructions,
                language.ToString().ToLowerInvariant(),
                methods,
                uriModel.RedirectUri,
                uriModel.CancelUri,
                uriModel.ErrorUri,
                this.vetumaUtilities.GetConfigUriKey(VetumaKeys.VetumaAuthenticationUrl),
                this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaAuthenticationSharedSecretId),
                this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaAuthenticationSharedSecret),
                this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaApplicationIdentifier),
                this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaAuthenticationConfigurationId));

            // Vetuma requires this additional parameter to retrieve personal ids from VTJ
            request.ExtraData = "VTJ1";

            // store a unique identifier and attach it to the request
            request.TransactionId = transactionId;
            request.ApplicationName = this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaApplicationDisplayName);

            this.vetumeService.SubmitVetumaAuthenticationRequest(request);
        }

        /// <summary>
        /// Determine the result of the Vetuma authentication by examining the returned request (user is returned to our service)
        /// </summary>
        /// <param name="transactionId">Unique transaction ID</param>
        /// <returns>VetumaAuthResponse model</returns>
        /// <remarks>Retrieves the current request internally from HttpContext</remarks>
        public VetumaAuthResponse ProcessResult(string transactionId)
        {
            VetumaAuthenticationResponse response =
                this.vetumeService.CreateVetumaAuthentificationResponse(
                this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaAuthenticationSharedSecretId),
                this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaAuthenticationSharedSecret));

            if (response == null)
            {
                throw new ArgumentException("Vetuma Authentification response is null");
            }

            bool responseValid = response.Validate();
            bool uniqueIdValid = transactionId == response.TransactionId;

            if ((response.Status == this.vetumaUtilities.GetConfigKey(VetumaKeys.SUCCESSFUL) && responseValid && uniqueIdValid) == false)
            {
                return null;
            }

            return new VetumaAuthResponse
            {
                FirstName = response.FirstName,
                LastName = response.LastName,
                Personid = response.PersonId
            };
        }
    }
}
