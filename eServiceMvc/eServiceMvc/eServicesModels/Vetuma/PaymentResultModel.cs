namespace Uma.Eservices.Models.Vetuma
{
    /// <summary>
    /// PaymentResultModel object is used to store payment response results
    /// </summary>
    public class PaymentResultModel
    {
        /// <summary>
        /// Was the payment successful or not
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Paid sum to show on UI
        /// </summary>
        public double PaidSum { get; set; }

        /// <summary>
        /// Eservice form application ID
        /// </summary>
        public int Applicationid { get; set; }

        /// <summary>
        /// The payment Id returned from Vetuma
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// The archiving code assigned to the payment and returned from Vetuma
        /// </summary>
        public string ArchivingCode { get; set; }

        /// <summary>
        /// The order number sent with the original request
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// The reference number sent with the original request
        /// </summary>
        public string ReferenceNumber { get; set; }
    }
}
