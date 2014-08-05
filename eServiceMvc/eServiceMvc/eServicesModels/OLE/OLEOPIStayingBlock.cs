namespace Uma.Eservices.Models.OLE
{
    /// <summary>
    /// Block on staying longer after studies reasoning
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLEOPI")]
    public class OLEOPIStayingBlock
    {
        /// <summary>
        /// Contains ID valuue of Application to be preserved between web calls
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Description of Longevity of studies
        /// </summary>
        public string DurationOfStudies { get; set; }

        /// <summary>
        /// In case applicant stays longer than actual studies - reasoning behind that
        /// </summary>
        public string ReasonToStayLonger { get; set; }

        /// <summary>
        /// Reson if applicant wants longer residence permit than study period
        /// </summary>
        public string ReasonToHaveLongerPermit { get; set; }

        /// <summary>
        /// Why applicant wants to study in Finland this specific area of studies of his
        /// </summary>
        public string ReasonToStudyInFinland { get; set; }

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
                int filledFields =
                    string.IsNullOrWhiteSpace(this.DurationOfStudies) ? 0 : 1;
                decimal fillPercentage = filledFields / CountOfRequiredInfoFields * 100;
                return (int)fillPercentage;
            }
        }
    }
}
