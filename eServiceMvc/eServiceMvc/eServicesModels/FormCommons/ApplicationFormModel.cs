namespace Uma.Eservices.Models.FormCommons
{
    using System.Collections.Generic;
    using Uma.Eservices.Models.OLE;

    /// <summary>
    /// General application from model.
    /// Contains all page models
    /// </summary>
    public class ApplicationFormModel
    {
        /// <summary>
        /// Unique Identifier of a form, should be 8-9 digits long identifier, also used to correlate form with UMA data
        /// </summary>
        public int ApplicationFormId { get; set; }

        /// <summary>
        /// Relation to User record who's application it is
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Enumeration of what Form type it is (OLE-OPI; KAN-1 etc.)
        /// </summary>
        public FormType FormCode { get; set; }

        /// <summary>
        /// Determines whether this form is an extension to previous full application
        /// </summary>
        public bool IsExtension { get; set; }

        /// <summary>
        /// Enumeration of what status /state this application currently is in.
        /// </summary>
        public short FormStatus { get; set; }

        /// <summary>
        /// List of all application attachments
        /// </summary>
        public List<Attachment> Attachments { get; set; }

        /// <summary>
        /// OLE OPI First Page
        /// </summary>
        public OLEPersonalInformationPage OleOpiPersonalInformationPge { get; set; }

        /// <summary>
        /// OLE OPI Secound page
        /// </summary>
        public OLEOPIEducationInformationPage OleOpiEducationInformationPage { get; set; }

        /// <summary>
        /// OLE OPI Third page
        /// </summary>
        public OLEOPIFinancialInformationPage OleOpiFinancialInformationPage { get; set; }
    }
}
