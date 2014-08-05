namespace Uma.Eservices.Models.OLE
{
    using System.Collections.Generic;

    using Uma.Eservices.Models.OLE.Enums;
    using Uma.Eservices.Models.Shared;
    using Uma.Eservices.Models.FormCommons;

    /// <summary>
    /// Common for most (all) OLE forms - personam (Customer) general information
    /// </summary>
    public class OLEAttachmentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEAttachmentPage"/> class.
        /// </summary>
        public OLEAttachmentPage()
        {
            this.Travel = new AttachmentBlock();
            this.Income = new AttachmentBlock();
            this.Health = new AttachmentBlock();
            this.Certificate = new AttachmentBlock();
            this.Registration = new AttachmentBlock();
            this.Degree = new AttachmentBlock();
            this.EmploymentCertificates = new AttachmentBlock();
            this.Refusal = new AttachmentBlock();
            this.Guardian = new AttachmentBlock();
            this.Supplemental = new AttachmentBlock();
        }

        /// <summary>
        /// Contains ID valuue of Application to be preserved between web calls
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Returns percentage of filling for this page object as average of separate block fills
        /// Returns number from 0 to 100 (%)
        /// </summary>
        public int PageFillPercentage
        {
            get
            {
                return 100;
            }
        }

        /// <summary>
        /// Object to draw and manage and retrieve form elements, location and filling statuses
        /// </summary>
        public FormProgressModel FormProgress { get; set; }

        /// <summary>
        /// Attachment for Tralev document (passport)
        /// </summary>
        public AttachmentBlock Travel { get; set; }

        /// <summary>
        /// Document proof of person income
        /// </summary>
        public AttachmentBlock Income { get; set; }

        /// <summary>
        /// Health insurance document
        /// </summary>

        public AttachmentBlock Health { get; set; }

        /// <summary>
        /// Certificate document
        /// </summary>
        public AttachmentBlock Certificate { get; set; }

        /// <summary>
        /// Registration in educational institution
        /// </summary>
        public AttachmentBlock Registration { get; set; }

        /// <summary>
        /// Current eduacational degree proof
        /// </summary>
        public AttachmentBlock Degree { get; set; }

        /// <summary>
        /// Any employemnt certificates attachment
        /// </summary>
        public AttachmentBlock EmploymentCertificates { get; set; }

        /// <summary>
        /// Refusal of entry document (if any)
        /// </summary>
        public AttachmentBlock Refusal { get; set; }

        /// <summary>
        /// Guardian? what is this?
        /// </summary>
        public AttachmentBlock Guardian { get; set; }

        /// <summary>
        /// Random any other document list
        /// </summary>
        public AttachmentBlock Supplemental { get; set; }
    }
}
