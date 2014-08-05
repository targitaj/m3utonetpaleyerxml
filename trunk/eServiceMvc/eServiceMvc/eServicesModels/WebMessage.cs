namespace Uma.Eservices.Models
{
    using System;

    /// <summary>
    /// Distinguishes the type of web message
    /// </summary>
    [Serializable]
    public enum WebMessageType
    {
        /// <summary>
        /// The informative (default) web message
        /// </summary>
        Informative = 0,

        /// <summary>
        /// Success (of operation) message
        /// </summary>
        Success = 1,

        /// <summary>
        /// Message(s) of errors happened during operation
        /// </summary>
        Error = 2
    }

    /// <summary>
    /// Class to hold system/web messages of any of types: Informative, Success (of operation) or Error.
    /// </summary>
    [Serializable]
    public class WebMessage
    {
        /// <summary>
        /// Gets or sets the type of the web message.
        /// </summary>
        public WebMessageType WebMessageType { get; set; }

        /// <summary>
        /// Title of the message - keep it brief and self explaining.
        /// Gets localized in current UI language
        /// </summary>
        public string MessageTitle { get; set; }

        /// <summary>
        /// Detailed description of message - further steps, explanation of what's wrong
        /// Gets localized in current UI language
        /// </summary>
        public string MessageDescription { get; set; }

        /// <summary>
        /// Time in seconds, after which message is getting automatically hidden.
        /// Specifying zero (0) will make message to show indefinite (manually close)
        /// </summary>
        public int AutohideSeconds { get; set; }

        /// <summary>
        /// Allows to add button with link to any resource
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "MVC doesn't use Uri for redirects")]
        public string LinkUrl { get; set; }

        /// <summary>
        /// Title for the link button.
        /// Gets localized in current UI language
        /// </summary>
        public string LinkTitle { get; set; }
    }
}