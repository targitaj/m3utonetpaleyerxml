namespace Uma.Eservices.Models.Dashboard
{
    using System.Collections.Generic;
    using Uma.Eservices.Models.FormCommons;

    /// <summary>
    /// View Model for suppliemental document to user application
    /// </summary>
    public class SupplementalDocumentModel : AttachmentBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SupplementalDocumentModel"/> class.
        /// </summary>
        public SupplementalDocumentModel()
        {
            this.Applications = new Dictionary<string, string>();
        }

        /// <summary>
        /// List of user applications for chooser
        /// </summary>
        public Dictionary<string, string> Applications { get; set; }

        /// <summary>
        /// Chosen application
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Temp property  used to replace -> DashBoard bool model
        /// </summary>
        public bool TempBool { get; set; }
    }
}
