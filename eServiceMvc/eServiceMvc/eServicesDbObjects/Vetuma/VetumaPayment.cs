namespace Uma.Eservices.DbObjects.Vetuma
{
    using System;

    /// <summary>
    /// DB Vetuma Payment model. Used to get / set Payment ralated data to and from DB
    /// </summary>
    public class VetumaPayment
    {
        /// <summary>
        /// Unique table payment id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Application form id
        /// </summary>
        public int ApplicationFormId { get; set; }

        /// <summary>
        /// Payment transaction Id
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// Amount of money has been paid
        /// </summary>
        public double PaidSum { get; set; }

        /// <summary>
        /// True ir application id paid, false if not
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// Vetuma order number  -> Vetuma set value
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Generated Vetuma reference number
        /// </summary>
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// Arch code   -> Vetuma set value
        /// </summary>
        public string ArchivingCode { get; set; }

        /// <summary>
        /// Vetuma payment id   -> Vetuma set value
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// Payment creation Date
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Payment date
        /// </summary>
        public DateTime? PaymentDate { get; set; }
    }
}
