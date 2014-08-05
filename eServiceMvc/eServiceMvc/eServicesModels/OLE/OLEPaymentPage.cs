namespace Uma.Eservices.Models.OLE
{
    using System.Collections.Generic;
    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// OLE payment page model
    /// </summary>
    public class OLEPaymentPage
    {
        /// <summary>
        /// Contains ID valuue of Application to be preserved between web calls
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Get of sets selected emabassy from EmbassyList
        /// </summary>
        public string SelectedEmbassy { get; set; }

        /// <summary>
        /// Object to draw and manage and retrieve form elements, location and filling statuses
        /// </summary>
        public FormProgressModel FormProgress { get; set; }

        /// <summary>
        /// Contains logic to determine on what percentage this data block is filled. 
        /// Should return number from 0 to 100 (%)
        /// </summary>
        public int BlockFillPercentage
        {
            get
            {
                // This may be replaced with validator-related logic
                return string.IsNullOrWhiteSpace(this.SelectedEmbassy) ? 0 : 1;
            }
        }
    }
}
