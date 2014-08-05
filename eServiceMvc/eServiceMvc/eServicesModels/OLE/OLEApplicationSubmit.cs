namespace Uma.Eservices.Models.OLE
{
    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// Model for OLE form application submit page
    /// </summary>
    public class OLEApplicationSubmit
    {
        /// <summary>
        /// Contains ID valuue of Application to be preserved between web calls
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Object to draw and manage and retrieve form elements, location and filling statuses
        /// </summary>
        public FormProgressModel FormProgress { get; set; }

        /// <summary>
        /// Bool Value. Un form user select that understand local laws
        /// </summary>
        public bool IUnderstand { get; set; }

        /// <summary>
        /// Contains logic to determine on what percentage this data block is filled. 
        /// Should return number from 0 to 100 (%)
        /// </summary>
        public int BlockFillPercentage
        {
            get
            {
                // This may be replaced with validator-related logic
                return this.IUnderstand ? 0 : 1;
            }
        }
    }
}
