namespace Uma.Eservices.Models.OLE
{
    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// Common for most (all) OLE forms - personam (Customer) general information
    /// </summary>
    public class OLEPersonalInformationPage : BaseModel
    {
        /// <summary>
        /// Default ctor. Init all ref object in model
        /// </summary>
        public OLEPersonalInformationPage()
        {
            this.PersonDataBlock = new OLEPersonalDataBlock();
            this.ContactInformationBlock = new OLEContactInfoBlock();
            this.PassportInformationBlock = new OLEPassportInformationBlock();
            this.ResidenceDurationBlock = new OLEResidenceDurationBlock();
            this.FamilyBlock = new OLEFamilyBlock();
        }

        /// <summary>
        /// OLEPersonalInformationPage unique Id
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
        /// Determines whether this form is an extension to previous full application
        /// </summary>
        public bool IsExtension { get; set; }

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
        /// Returns percentage of filling for this page object as average of separate block fills
        /// Returns number from 0 to 100 (%)
        /// </summary>
        //public int PageFillPercentage
        //{
        //    get
        //    {
        //        // TODO: Finalize upon creating all blocks
        //        const decimal BlockCount = 4;
        //        var fillRate =
        //            this.PersonDataBlock.BlockFillPercentage +
        //            this.ContactInformationBlock.BlockFillPercentage +
        //            this.PassportInformationBlock.BlockFillPercentage +
        //            this.ResidenceDurationBlock.BlockFillPercentage;
        //        var averageFillPercentage = fillRate / BlockCount;
        //        return (int)averageFillPercentage;
        //    }
        //}
    }
}
