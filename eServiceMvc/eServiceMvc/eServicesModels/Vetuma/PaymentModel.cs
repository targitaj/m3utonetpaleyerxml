namespace Uma.Eservices.Models.Vetuma
{
    using System.Collections.Generic;
    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// PaymentModel object model Used for View fields
    /// </summary>
    public class PaymentModel
    {
        /// <summary>
        /// Payment Amount
        /// </summary>
        public double? PayableAmount { get; set; }

        /// <summary>
        /// Verifeis is application is paid of not
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// Eservice form application ID
        /// </summary>
        public int Applicationid { get; set; }
    }
}
