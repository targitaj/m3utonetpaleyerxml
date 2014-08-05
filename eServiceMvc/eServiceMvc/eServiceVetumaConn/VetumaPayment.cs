namespace Uma.Eservices.VetumaConn
{
    /// <summary>
    /// PaymentRequest object model
    /// Model contains necessary fields to make valid payment in Vetuma services
    /// </summary>
    public class PaymentRequest
    {
        /// <summary>
        /// Payment Amount
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Payment OrderNumber
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// Payment reference number see method: FormatReferenceNumber
        /// </summary>
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// Translated text for payment description
        /// </summary>
        public string PaymentDescription { get; set; }

        /// <summary>
        /// Translated text for seller
        /// </summary>
        public string MessageToSeller { get; set; }

        /// <summary>
        /// Transaction lagugage 2-letter lang. code EN, FI, SV
        /// </summary>
        public TransactionLanguage Language { get; set; }

        /// <summary>
        /// Verifies if payment should go to police of not
        /// </summary>
        public bool DirectToPolice { get; set; }

        /// <summary>
        /// Uri links for case of error, or case if user cancel process
        /// </summary>
        public VetumaUriModel UriLinks { get; set; }

        /// <summary>
        /// Translated text for redirect buttons
        /// </summary>
        public string VetumaRedirectButtonText { get; set; }

        /// <summary>
        /// Translated text for redirect text button
        /// </summary>
        public string VetumaRedirectTextMessage { get; set; }

        /// <summary>
        /// Unique  id taht describes authorization process
        /// </summary>
        public string TransactionId { get; set; }
    }
}
