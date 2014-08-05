namespace Uma.Eservices.Web.Core
{
    using System;
    using System.Collections.Generic;
    using Uma.Eservices.Models;

    /// <summary>
    /// Use this interface for class to manage Web messages in controller(s) to show to user after page reload, like
    /// success, error or simple informative messages populated during controller code run
    /// </summary>
    public interface IWebMessages
    {
        /// <summary>
        /// Gets the messages from object. Getting messagaes will clear them for further retrieval.
        /// </summary>
        List<WebMessage> Messages { get; }

        /// <summary>
        /// Adds the error message to messages.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="linkUrl">The link URL.</param>
        /// <param name="linkTitle">The link title.</param>
        /// <param name="closeTimeout">The close timeout.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#", Justification = "MVC doesn't support Uri natively")]
        void AddErrorMessage(string title, string description = null, string linkUrl = null, string linkTitle = null, int closeTimeout = 0);

        /// <summary>
        /// Adds the information message.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="linkUrl">The link URL.</param>
        /// <param name="linkTitle">The link title.</param>
        /// <param name="closeTimeout">The close timeout.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#", Justification = "MVC doesn't support Uri natively")]
        void AddInfoMessage(string title, string description = null, string linkUrl = null, string linkTitle = null, int closeTimeout = 0);

        /// <summary>
        /// Adds the success message.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <param name="closeTimeout">The close timeout.</param>
        void AddSuccessMessage(string title, string description = null, int closeTimeout = 0);
    }
}