namespace Uma.Eservices.Web.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Web;
    using Uma.Eservices.Models;

    /// <summary>
    /// Use this class to manage Web messages in controller(s) to show to user after page reload, like
    /// success, error or simple informative messages populated during controller code run
    /// </summary>
    [DebuggerDisplay("WebMessages:{Messages.Count}")]
    public class WebMessages : IWebMessages
    {
        /// <summary>
        /// Field to hold persistence medium for messages, like runtime - TempData, otherwise - any mocked TempDataDictionary object.
        /// </summary>
        private readonly HttpSessionStateBase sessionData;

        /// <summary>
        /// Gets the web message collection from session (or mock).
        /// </summary>
        private List<WebMessage> WebMessageCollection
        {
            get
            {
                if (this.sessionData == null)
                {
                    return new List<WebMessage>();
                }

                if (this.sessionData["__webMessages"] == null)
                {
                    this.sessionData["__webMessages"] = new List<WebMessage>();
                }

                return (List<WebMessage>)this.sessionData["__webMessages"];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebMessages"/> class.
        /// </summary>
        /// <param name="sessionData">The temporary data implementation. From controller - supply TempData object</param>
        public WebMessages(HttpSessionStateBase sessionData)
        {
            this.sessionData = sessionData;
        }

        /// <summary>
        /// Returns messages saved before redirect.
        /// Messages are cleared after they are read via this metthod (next call will return empty list)
        /// </summary>
        /// <value>List of messages or empty list if not set or already read.</value>
        public List<WebMessage> Messages
        {
            get
            {
                if (this.sessionData == null || this.sessionData["__webMessages"] == null)
                {
                    return new List<WebMessage>();
                }

                List<WebMessage> messages = (List<WebMessage>)this.sessionData["__webMessages"];
                this.sessionData.Remove("__webMessages");
                if (messages == null)
                {
                    return new List<WebMessage>();
                }

                return messages;
            }
        }

        /// <summary>
        /// Adds the information message to pool of messages.
        /// </summary>
        /// <param name="title">The title - mandatory title text, must be short and descriptive.</param>
        /// <param name="description">The description - optional description of information.</param>
        /// <param name="linkUrl">The link URL, to include button with <paramref name="linkTitle"/>.</param>
        /// <param name="linkTitle">The link title - for button with <paramref name="linkUrl"/>.</param>
        /// <param name="closeTimeout">The close timeout (in seconds) for message to self-close. 0 (default) - message stays until closed manually or until next page</param>
        public void AddInfoMessage(string title, string description = null, string linkUrl = null, string linkTitle = null, int closeTimeout = 0)
        {
            // TODO: Localization of Titles and Description
            WebMessage webMessage = new WebMessage
            {
                MessageTitle = title,
                MessageDescription = description,
                AutohideSeconds = closeTimeout,
                WebMessageType = WebMessageType.Informative,
                LinkUrl = linkUrl,
                LinkTitle = linkTitle
            };
            this.WebMessageCollection.Add(webMessage);
        }

        /// <summary>
        /// Adds the success message to pool of messages.
        /// </summary>
        /// <param name="title">The title - mandatory title text, must be short and descriptive.</param>
        /// <param name="description">The description - optional description of information.</param>
        /// <param name="closeTimeout">The close timeout (in seconds) for message to self-close. 0 (default) - message stays until closed manually or until next page</param>
        public void AddSuccessMessage(string title, string description = null, int closeTimeout = 0)
        {
            // TODO: Localization of Titles and Description
            WebMessage webMessage = new WebMessage
            {
                MessageTitle = title,
                MessageDescription = description,
                AutohideSeconds = closeTimeout,
                WebMessageType = WebMessageType.Success
            };
            this.WebMessageCollection.Add(webMessage);
        }

        /// <summary>
        /// Adds the error message to pool of messages.
        /// </summary>
        /// <param name="title">The title - mandatory title text, must be short and descriptive.</param>
        /// <param name="description">The description - optional description of information.</param>
        /// <param name="linkUrl">The link URL, to include button with <paramref name="linkTitle"/>.</param>
        /// <param name="linkTitle">The link title - for button with <paramref name="linkUrl"/>.</param>
        /// <param name="closeTimeout">The close timeout (in seconds) for message to self-close. 0 (default) - message stays until closed manually or until next page</param>
        public void AddErrorMessage(string title, string description = null, string linkUrl = null, string linkTitle = null, int closeTimeout = 0)
        {
            // TODO: Localization of Titles and Description
            WebMessage webMessage = new WebMessage
            {
                MessageTitle = title,
                MessageDescription = description,
                AutohideSeconds = closeTimeout,
                WebMessageType = WebMessageType.Error,
                LinkUrl = linkUrl,
                LinkTitle = linkTitle
            };
            this.WebMessageCollection.Add(webMessage);
        }
    }
}