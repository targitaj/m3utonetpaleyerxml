namespace Uma.Eservices.VetumaConn
{
    using System;
    using System.Collections.ObjectModel;
    using Fujitsu.Vetuma.Toolkit;

    /// <summary>
    /// IPaymentService interface implenentation. 
    /// Used to make payments and process payments results
    /// </summary>
    public class PaymentService : IPaymentService
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
        public PaymentService(IVetumaService vetumaService, IVetumaUtilities vetumaUtilities)
        {
            this.vetumeService = vetumaService;
            this.vetumaUtilities = vetumaUtilities;
        }

        /// <summary>
        /// Redirects the current user to the Vetuma payment interface
        /// </summary>
        /// <param name="paymentModel">Object model that contains necessary fields for payment process</param>
        public void MakePayment(PaymentRequest paymentModel)
        {
            Collection<VetumaPaymentMethod> methods = new Collection<VetumaPaymentMethod>();
            methods.Add(VetumaPaymentMethod.InternetBank);

            if (!paymentModel.DirectToPolice)
            {
                methods.Add(VetumaPaymentMethod.CreditCard);
            }

            var request = new VetumaPaymentRequest(
                paymentModel.VetumaRedirectButtonText,
                paymentModel.VetumaRedirectTextMessage,
                paymentModel.Language.ToString().ToLowerInvariant(),
                methods,
                paymentModel.UriLinks.RedirectUri,
                paymentModel.UriLinks.CancelUri,
                paymentModel.UriLinks.ErrorUri,
                this.vetumaUtilities.GetConfigUriKey(VetumaKeys.VetumaPaymentUrl),
                paymentModel.DirectToPolice ?
                  this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaPolicePaymentSharedSecretId)
                : this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaPaymentSharedSecretId),

                paymentModel.DirectToPolice ?
                  this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaPolicePaymentSharedSecret)
                : this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaPaymentSharedSecret),

                paymentModel.DirectToPolice ?
                  this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaPoliceApplicationIdentifier)
                : this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaApplicationIdentifier),

                paymentModel.DirectToPolice ?
                  this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaPolicePaymentConfigurationId)
                : this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaPaymentConfigurationId),

                paymentModel.Amount)
                {
                    OrderNumber = paymentModel.OrderNumber,
                    ReferenceNumber = paymentModel.ReferenceNumber,
                    MessageForm = paymentModel.PaymentDescription,
                    MessageSeller = paymentModel.PaymentDescription,
                    TransactionId = paymentModel.TransactionId,
                    ApplicationName = this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaApplicationDisplayName)
                };

            this.vetumeService.SubmitPaymentRequest(request);
        }

        /// <summary>
        /// Method that validates payment results. Do request to payment services use TransactionId as identifier for pay application
        /// </summary>
        /// <param name="transactionId">TransactionId unique payment identifier</param>
        /// <returns>PaymentResult object with payment response info</returns>
        public PaymentResult ProcessResult(string transactionId)
        {
            PaymentResult result = new PaymentResult();

            // construct new response object which will internally fill itself from HttpContext.Current.Request
            VetumaPaymentResponse response = this.vetumeService.CreateVetumaPaymentResponse(
                this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaPaymentSharedSecretId),
                this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaPaymentSharedSecret));

            // validate response for first payment account
            bool responseValid = this.vetumeService.PaymentResponseValidate(response);

            if (!responseValid)
            {
                // validate second payment account
                response = this.vetumeService.CreateVetumaPaymentResponse(
                this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaPolicePaymentSharedSecretId),
                this.vetumaUtilities.GetConfigKey(VetumaKeys.VetumaPolicePaymentSharedSecret));

                responseValid = this.vetumeService.PaymentResponseValidate(response);
            }

            if (response == null)
            {
                throw new ArgumentException("Vetuma response is null");
            }

            bool uniqueIdValid = transactionId == response.TransactionId;

            result.OrderNumber = response.OrderNumber;
            result.ReferenceNumber = response.ReferenceNumber;
            result.ArchivingCode = response.ArchivingCode;
            result.PaymentId = response.PaymentId;
            result.Success = response.Status == VetumaKeys.SUCCESSFUL.ToString().ToUpperInvariant()
                                                && responseValid && uniqueIdValid;
            result.TransactionId = transactionId;

            return result;
        }
    }
}