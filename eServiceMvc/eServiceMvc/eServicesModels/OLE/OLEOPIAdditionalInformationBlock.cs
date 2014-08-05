namespace Uma.Eservices.Models.OLE
{
    /// <summary>
    /// Very simple block of free text about additional information on ResPer Study form
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLEOPI")]
    public class OLEOPIAdditionalInformationBlock
    {
        /// <summary>
        /// Additional informaion free-text field
        /// </summary>
        public string AdditionalInformation { get; set; }

        /// <summary>
        /// Contains logic to determine on what percentage this data block is filled. 
        /// Should return number from 0 to 100 (%)
        /// </summary>
        public int BlockFillPercentage
        {
            get
            {
                // This may be replaced with validator-related logic
                const decimal CountOfRequiredInfoFields = 1;
                int filledFields = string.IsNullOrWhiteSpace(this.AdditionalInformation) ? 0 : 1;
                decimal fillPercentage = filledFields / CountOfRequiredInfoFields * 100;
                return (int)fillPercentage;
            }
        }
    }
}
