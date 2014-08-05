namespace Uma.Eservices.Models.Vetuma
{
    /// <summary>
    /// VetumaTextModel object contains necessary text fields for Vetuma auth/Payment process
    /// </summary>
    public class VetumaTextModel
    {
        /// <summary>
        /// Translated text for payment description (for payment process)
        /// </summary>
        public string PaymentDescription { get; set; }

        /// <summary>
        /// Translated text for seller (for payment process)
        /// </summary>
        public string MessageToSeller { get; set; }

        /// <summary>
        /// Vetuma button text (for auth process)
        /// </summary>
        public string VetumaButtonText { get; set; }

        /// <summary>
        /// Vetuma button instruction text  (for auth process)
        /// </summary>
        public string VetumaButtonInstructions { get; set; }
    }
}
