namespace Uma.Eservices.DbObjects
{
    using System;
    using System.Collections.Generic;

    using Uma.Eservices.DbObjects.FormCommons;
    using Uma.Eservices.DbObjects.OLE;

    /// <summary>
    /// Holds data about application - who, which, what status etc.
    /// </summary>
    public class ApplicationForm
    {
        /// <summary>
        /// Unique Identifier of a form, should be 8-9 digits long identifier, also used to correlate form with UMA data
        /// </summary>
        public virtual int ApplicationFormId { get; set; }

        /// <summary>
        /// Relation to User record who's application it is
        /// </summary>
        public virtual int UserId { get; set; }

        /// <summary>
        /// Enumeration of what Form type it is (OLE-OPI; KAN-1 etc.)
        /// </summary>
        public virtual FormType FormCode { get; set; }

        /// <summary>
        /// Determines whether this form is an extension to previous full application
        /// </summary>
        public virtual bool IsExtension { get; set; }

        /// <summary>
        /// Enumeration of what status /state this application currently is in.
        /// </summary>
        public virtual FormStatus FormStatus { get; set; }

        /// <summary>
        /// List of all application attachments
        /// </summary>
        public virtual List<Attachment> Attachments { get; set; }

        /// <summary>
        /// OLE OPI First Page
        /// </summary>
        public virtual List<OLEPersonalInformationPage> OleOpiPersonalInformationPage { get; set; }

        /// <summary>
        /// OLE OPI Secound page
        /// </summary>
        public virtual List<OLEOPIEducationInformationPage> OleOpiEducationInformationPage { get; set; }

        /// <summary>
        /// OLE OPI Third page
        /// </summary>
        public virtual List<OLEOPIFinancialInformationPage> OleOpiFinancialInformationPage { get; set; }
    }
}
