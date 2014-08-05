namespace Uma.Eservices.Models.Vetuma
{
    using Uma.Eservices.Models.Localization;

    /// <summary>
    /// VetumaPaymentModel contains necessary fields to create valid request for payment action
    /// </summary>
    public class VetumaPaymentModel
    {
        /// <summary>
        /// Eservice application Id
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Sum that need to pay
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Language that will be provided in Vetuma service
        /// </summary>
        public SupportedLanguage Language { get; set; }

        /// <summary>
        /// Uri links for case of error, or cancel of success
        /// </summary>
        public VetumaUriModel UriLinks { get; set; }

        /// <summary>
        /// Unique Transaction ID
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// AuthorityOffice Label 
        /// </summary>
        public string AuthorityOfficeLabel { get; set; }
    }
}