namespace Uma.Eservices.Models.FormCommons
{
    using System.Collections.Generic;

    /// <summary>
    /// Model for OLE Form attachment block
    /// </summary>
    public class AttachmentBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentBlock"/> class.
        /// </summary>
        public AttachmentBlock()
        {
            this.Attachments = new List<Attachment>();
        }

        /// <summary>
        /// Dictionary of attachments
        /// </summary>
        public List<Attachment> Attachments { get; set; }

        /// <summary>
        /// Main application Id
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Description of an attacment
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Name of a document
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// File server name
        /// </summary>
        public string ServerFileName { get; set; }
    }
}
