namespace Uma.Eservices.VetumaConn
{
    /// <summary>
    /// Vetuma PaymentResult object model
    /// </summary>
    public class PaymentResult
    {
        /// <summary>
        /// Was the payment successful or not
        /// </summary>
        public bool Success { get; set; }

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

        /// <summary>
        /// The reference number for TransactionId
        /// </summary>
        public string TransactionId { get; set; }
    }
}
