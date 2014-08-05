namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Globalization;
    using System.Web;

    /// <summary>
    /// The mvc file save.
    /// </summary>
    public class FileSaveModel
    {
        /// <summary>
        /// Gets or sets file for saving.
        /// </summary>
        public HttpPostedFileBase File { get; set; }

        /// <summary>
        /// Gets or sets the directory for file saving.
        /// </summary>
        public string StorageDirectory { get; set; }

        /// <summary>
        /// Gets or sets the url for file access.
        /// </summary>
        public Uri UrlPrefix { get; set; }

        /// <summary>
        /// Gets or sets the file delete url.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "can contains action url"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", MessageId = "FileSaveModel", Justification = "can contains action url")]
        public string DeleteUrl { get; set; }

        /// <summary>
        /// Gets or sets the file time stamp for random file name.
        /// </summary>
        public DateTime FileTimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the name of the file to be saved if not set, <see cref="FileSaver"/> will generate a filename suffixed with filetimestamp.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="FileSaver"/> exceptions should be thrown or set any exception message in the error property which is default.
        /// </summary>
        /// <value>
        ///   <c>true</c> if <see cref="FileSaver"/> should throw exceptions; otherwise, <c>false</c>.
        /// </value>
        public bool ThrowExceptions { get; set; }

        /// <summary>
        /// The add file uri param to delete url to have ability delete files.
        /// </summary>
        /// <param name="paramName">
        /// The parameter name for deletion.
        /// </param>
        /// <param name="fileUrl">
        /// The url to file
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "can contains action url"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "FileSaveModel", Justification = "can contains action url")]
        public void AddFileUriParamToDeleteUrl(string paramName, string fileUrl)
        {
            if (!string.IsNullOrEmpty(this.DeleteUrl))
            {
                // means has query
                if (this.DeleteUrl.Contains("?") && !this.DeleteUrl.Contains("&" + paramName))
                {
                    this.DeleteUrl += string.Format(CultureInfo.InvariantCulture, "&{0}={1}", paramName, fileUrl);
                }
                else
                {
                    this.DeleteUrl += string.Format(CultureInfo.InvariantCulture, "?{0}={1}", paramName, fileUrl);
                }                    
            }                            
        }
    }
}
