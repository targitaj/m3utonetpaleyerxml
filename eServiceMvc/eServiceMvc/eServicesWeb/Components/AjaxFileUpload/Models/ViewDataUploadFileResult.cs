namespace Uma.Eservices.Web.Components
{
    using System;

    /// <summary>
    /// The view data upload file result.
    /// </summary>
    public class ViewDataUploadFileResult
    {
        // for JQuery file upload

        /// <summary>
        /// Gets or sets the error during file upload.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the file size.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets the file type.
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// Gets or sets the file url.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "used in ajax"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", MessageId = "Url", Justification = "used in ajax")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the url for file deletion.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "used in ajax"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", MessageId = "DeleteUrl", Justification = "used in ajax")]
        public string DeleteUrl { get; set; }

        /// <summary>
        /// Gets or sets the file thumbnail url.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "used in ajax"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", MessageId = "ThumbnailUrl", Justification = "used in ajax")]
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the delete type. GET or POST
        /// </summary>
        public string DeleteType { get; set; }

        // for use from any controller and/or views

        /// <summary>
        /// Gets or sets the full path to file.
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Gets or sets the saved file name.
        /// </summary>
        public string SavedFileName { get; set; }

        // for storing

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the document name.
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
    }
}
