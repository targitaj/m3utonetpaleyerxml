namespace Uma.Eservices.Models.OLE
{
    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// OLEOPIEducationInformationPage model
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLEOPI")]
    public class OLEOPIEducationInformationPage
    {
        /// <summary>
        /// Default ctor. Init all ref object in model
        /// </summary>
        public OLEOPIEducationInformationPage()
        {
            this.EducationInstitution = new OLEOPIEducationInstitutionBlock();
            this.StayingLongerResoning = new OLEOPIStayingBlock();
            this.PreviousStudiesAndWork = new OLEOPIPreviousStudiesBlock();
        }

        /// <summary>
        /// OLEOPIEducationInformationPage unique Id
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
        /// Block of information about Education Institution and Study types
        /// </summary>
        public OLEOPIEducationInstitutionBlock EducationInstitution { get; set; }

        /// <summary>
        /// Block of information on reasons why applicant would like to stay longer or have longer ResPerm
        /// </summary>
        public OLEOPIStayingBlock StayingLongerResoning { get; set; }

        /// <summary>
        /// Block to describe any previous studies of applicant and work experience in this field
        /// </summary>
        public OLEOPIPreviousStudiesBlock PreviousStudiesAndWork { get; set; }

        /// <summary>
        /// Returns percentage of filling for this page object as average of separate block fills
        /// Returns number from 0 to 100 (%)
        /// </summary>
        public int PageFillPercentage
        {
            get
            {
                // TODO: Finalize upon creating all blocks
                const decimal BlockCount = 3;
                var fillRate = this.EducationInstitution.BlockFillPercentage +
                    this.StayingLongerResoning.BlockFillPercentage +
                    this.PreviousStudiesAndWork.BlockFillPercentage;
                var averageFillPercentage = fillRate / BlockCount;
                return (int)averageFillPercentage;
            }
        }
    }
}
