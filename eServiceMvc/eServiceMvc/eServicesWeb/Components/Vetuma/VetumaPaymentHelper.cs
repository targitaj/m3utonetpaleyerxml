namespace Uma.Eservices.Web.Components.Vetuma
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using Uma.Eservices.Logic.Features.VetumaService;
    using Uma.Eservices.Models.Vetuma;

    /// <summary>
    /// This class contains common helper methods for Vetuma payment and payment validation processes
    /// </summary>
    public class VetumaPaymentHelper
    {
        /// <summary>
        /// IVetumaPaymentLogic property
        /// </summary>
        [Dependency]
        public IVetumaPaymentLogic PaymentLogic { get; set; }

        /// <summary>
        /// Class contrucort. set Payment logic class
        /// </summary>
        /// <param name="paymentLogic">IVetumaPaymentLogic instance</param>
        public VetumaPaymentHelper(IVetumaPaymentLogic paymentLogic)
        {
            this.PaymentLogic = paymentLogic;
        }

        /// <summary>
        /// Used to do payment. Method redirects user  to Vetuma payment page
        /// Method contains links in case of success, error
        /// </summary>
        /// <param name="applicationId">Form application ID</param>
        /// <param name="selectedEmbassy">Selected embassy</param>
        public void SubmitPayment(int applicationId, string selectedEmbassy)
        {
            if (applicationId == 0) { throw new ArgumentException("applicationId is 0"); }

            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var baseUrl = urlHelper.Content("~");
            Uri errorCancelUri = new System.Uri(@HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + baseUrl);
            Uri redirectUri = new System.Uri(@HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + baseUrl + "ole/opi/" + applicationId + "/Step7");

            this.PaymentLogic.MakePayment(new VetumaPaymentModel
            {
                ApplicationId = applicationId,
                AuthorityOfficeLabel = selectedEmbassy,
                UriLinks = new VetumaUriModel
                {
                    CancelUri = errorCancelUri,
                    ErrorUri = errorCancelUri,
                    RedirectUri = redirectUri
                }
            });
        }

        /// <summary>
        /// Method that is responsible for payment validation.
        /// Validates payment after Vetume page redirect
        /// </summary>
        /// <param name="applicationId">Form application ID</param>
        /// <returns>Bool value of success or error result</returns>
        public bool ProcessResult(int applicationId)
        {
            var result = this.PaymentLogic.ProcessResult(applicationId);

            return result.IsPaid;
        }

        /// <summary>
        /// Checks if application is already paid.
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <returns>True if paid, false if not</returns>
        public bool IsApplicationPaid(int applicationId)
        {
            return this.PaymentLogic.IsApplicationPaid(applicationId);
        }
    }
}