namespace Uma.Eservices.Models.FormCommons
{
    using Uma.Eservices.Models.OLE.Enums;

    /// <summary>
    /// Model part for setting up attachment data for supplemental documents
    /// </summary>
    public class Attachment
    {
        /// <summary>
        /// Description of an attacment
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Name of a document
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// Actual file name of attachment
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// File name which is stored(uploaded) on server
        /// </summary>
        public string ServerFileName { get; set; }

        /// <summary>
        /// Attachment type
        /// </summary>
        public AttachmentTypeEnum AttachmentType { get; set; }
    }
}
