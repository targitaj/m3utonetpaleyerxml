namespace Uma.Eservices.Models.OLE
{
    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// OLEOPIFinancialInformationPage model
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLEOPI")]
    public class OLEOPIFinancialInformationPage
    {
        /// <summary>
        /// Default ctor. Init all ref object in model
        /// </summary>
        public OLEOPIFinancialInformationPage()
        {
            this.FinancialStudySupport = new OLEOPIFinancialSupportBlock();
            this.HealthInsurance = new OLEOPIHealthInsuranceBlock();
            this.AdditionalInformation = new OLEOPIAdditionalInformationBlock();
            this.CriminalInformation = new OLEOPICriminalInfoBlock();
        }

        /// <summary>
        /// OLEOPIFinancialInformationPage unique Id
        /// </summary>
        public int PageId { get; set; }

        /// <summary>
        /// Contains ID valuue of Application to be preserved between web calls
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Object to draw and manage and retrieve form elements, location and filling statuses
        /// </summary>
        public FormProgressModel FormProgress { get; set; }

        /// <summary>
        /// Block for financial support of studies
        /// </summary>
        public OLEOPIFinancialSupportBlock FinancialStudySupport { get; set; }

        /// <summary>
        /// Applicant health insurance information block
        /// </summary>
        public OLEOPIHealthInsuranceBlock HealthInsurance { get; set; }

        /// <summary>
        /// Additional information block (free-text field)
        /// </summary>
        public OLEOPIAdditionalInformationBlock AdditionalInformation { get; set; }

        /// <summary>
        /// Block on applicant criminal information
        /// </summary>
        public OLEOPICriminalInfoBlock CriminalInformation { get; set; }

        /// <summary>
        /// Returns percentage of filling for this page object as average of separate block fills
        /// Returns number from 0 to 100 (%)
        /// </summary>
        public int PageFillPercentage
        {
            get
            {
                // TODO: Finalize upon creating all blocks
                const decimal BlockCount = 4;
                var fillRate = this.FinancialStudySupport.BlockFillPercentage +
                    this.HealthInsurance.BlockFillPercentage +
                    this.AdditionalInformation.BlockFillPercentage +
                    this.CriminalInformation.BlockFillPercentage;
                var averageFillPercentage = fillRate / BlockCount;
                return (int)averageFillPercentage;
            }
        }
    }
}
