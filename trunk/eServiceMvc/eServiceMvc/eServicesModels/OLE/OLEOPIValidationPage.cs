namespace Uma.Eservices.Models.OLE
{
    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// Common for most (all) OLE forms - personam (Customer) general information
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLEOPI")]
    public class OLEOPIValidationPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEOPIValidationPage"/> class.
        /// </summary>
        public OLEOPIValidationPage()
        {
            this.PersonDataBlock = new OLEPersonalDataBlock();
            this.ContactInformationBlock = new OLEContactInfoBlock();
            this.PassportInformationBlock = new OLEPassportInformationBlock();
            this.ResidenceDurationBlock = new OLEResidenceDurationBlock();
            this.FamilyBlock = new OLEFamilyBlock();
            this.EducationInstitution = new OLEOPIEducationInstitutionBlock();
            this.StayingLongerResoning = new OLEOPIStayingBlock();
            this.PreviousStudiesAndWork = new OLEOPIPreviousStudiesBlock();
            this.FinancialStudySupport = new OLEOPIFinancialSupportBlock();
            this.HealthInsurance = new OLEOPIHealthInsuranceBlock();
            this.AdditionalInformation = new OLEOPIAdditionalInformationBlock();
            this.CriminalInformation = new OLEOPICriminalInfoBlock();
        }

        /// <summary>
        /// Contains ID valuue of Application to be preserved between web calls
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Object to draw and manage and retrieve form elements, location and filling statuses
        /// </summary>
        public FormProgressModel FormProgress { get; set; }

        /// <summary>
        /// Block of Personal information of an applicant
        /// </summary>
        public OLEPersonalDataBlock PersonDataBlock { get; set; }

        /// <summary>
        /// Block of Contact information of an applicant
        /// </summary>
        public OLEContactInfoBlock ContactInformationBlock { get; set; }

        /// <summary>
        /// Block on information about passport or other valid person document
        /// </summary>
        public OLEPassportInformationBlock PassportInformationBlock { get; set; }

        /// <summary>
        /// Block to describe arrival/departure dates and staying intentions
        /// </summary>
        public OLEResidenceDurationBlock ResidenceDurationBlock { get; set; }

        /// <summary>
        /// Block to submit information about family - spouse and children
        /// </summary>
        public OLEFamilyBlock FamilyBlock { get; set; }

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
                const decimal BlockCount = 11;
                var fillRate = 
                    //this.PersonDataBlock.BlockFillPercentage +
                    //this.ContactInformationBlock.BlockFillPercentage +
                    //this.PassportInformationBlock.BlockFillPercentage +
                    //this.ResidenceDurationBlock.BlockFillPercentage +
                    this.EducationInstitution.BlockFillPercentage + 
                    this.StayingLongerResoning.BlockFillPercentage + 
                    this.PreviousStudiesAndWork.BlockFillPercentage +
                    this.FinancialStudySupport.BlockFillPercentage +
                    this.HealthInsurance.BlockFillPercentage +
                    this.AdditionalInformation.BlockFillPercentage +
                    this.CriminalInformation.BlockFillPercentage;
                var averageFillPercentage = fillRate / BlockCount;
                return (int)averageFillPercentage;
            }
        }
    }
}
