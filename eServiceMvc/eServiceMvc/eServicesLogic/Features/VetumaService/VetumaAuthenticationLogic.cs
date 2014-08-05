namespace Uma.Eservices.Logic.Features.VetumaService
{
    using System;
    using System.Globalization;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Account;
    using Uma.Eservices.VetumaConn;
    using model = Uma.Eservices.Models.Vetuma;

    /// <summary>
    /// IVetumaAuthenticationLogic imlementation. 
    /// </summary>
    public class VetumaAuthenticationLogic : VetumaBaseHelper, IVetumaAuthenticationLogic
    {
        /// <summary>
        /// IStrongAuthenticationService service property
        /// </summary>
        private IStrongAuthenticationService StrongAuthentication { get; set; }

        /// <summary>
        /// VetumaAuthenticationLogic constuctor
        /// </summary>
        /// <param name="authService">IStrongAuthenticationService instance used to call Vetuma connector</param>
        /// <param name="localManager">ILocalizationManager instance used to translate Button texts</param>
        public VetumaAuthenticationLogic(IStrongAuthenticationService authService, ILocalizationManager localManager)
            : base(localManager)
        {
            this.StrongAuthentication = authService;
        }

        /// <summary>
        /// Authenticate creates, fill required fields for Vetuma auth action
        /// </summary>
        /// <param name="trancationId">Unique transaction Id</param>
        /// <param name="uriModel">Uri model that contains uris for Error/cancel/sucess case</param>
        public void Authenticate(string trancationId, model.VetumaUriModel uriModel)
        {
            var uris = VetumaServiceModelMapper.ToDBVetumaUriModel(uriModel);
            TransactionLanguage lang = VetumaServiceModelMapper.ToDbTransactionLanguage(Globalizer.CurrentUICultureLanguage.Value);
            this.StrongAuthentication.Authenticate(lang, uris, trancationId, base.TxtModel.VetumaButtonText, base.TxtModel.VetumaButtonInstructions);
        }

        /// <summary>
        /// Validates auth response and response results
        /// </summary>
        /// <param name="transactionId">Unique transaction Id</param>
        /// <returns>Application user</returns>
        public WebUser ProcessAuthenticationResult(string transactionId)
        {
            VetumaAuthResponse response = this.StrongAuthentication.ProcessResult(transactionId);
            if (response == null)
            {
                return null;
            }

            return this.PrefillUser(response);
        }

        /// <summary>
        /// Validates returned user from Vetuma auth process. Updates existing or create new if not found
        /// </summary>
        /// <param name="user">VetumaAuthResponse model</param>
        /// <returns>ApplicationUser object</returns>
        private WebUser PrefillUser(VetumaAuthResponse user)
        {
            WebUser returnVal = new WebUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PersonCode = user.Personid,
                IsStronglyAuthenticated = true,
                BirthDate = this.CalculateBirthDate(user.Personid)
            };

            return returnVal;
        }

        /// <summary>
        /// Calculates peron BirthDate from Person ID that was returned from Vetuma service
        /// </summary>
        /// <param name="personId">Person Id (should come from Vetuma response)</param>
        /// <returns>Person BirthDate</returns>
        private DateTime CalculateBirthDate(string personId)
        {
            // TO DO  will be necessary in future 100%  p.s. Valdis
            int year = int.Parse(personId.Substring(4, 2), CultureInfo.InvariantCulture);
            int day = int.Parse(personId.Substring(0, 2), CultureInfo.InvariantCulture);
            int month = int.Parse(personId.Substring(2, 2), CultureInfo.InvariantCulture);

            string separator = personId.Substring(6, 1);

            int century;

            switch (separator)
            {
                case "+":
                    century = 1800;
                    break;

                case "-":
                    century = 1900;
                    break;

                case "a":
                case "A":
                    century = 2000;
                    break;

                default:
                    throw new InvalidOperationException("Invalid personal Id");
            }

            return new DateTime(century + year, month, day);
        }
    }
}