namespace Uma.Eservices.Models.Home
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// View-Model to supply data for WebMessages partial view.
    /// This Mode/View is responsible for showing system messages to user.
    /// All message collections are initialized when class object is created
    /// </summary>
    public class WebMessagesModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebMessagesModel"/> class.
        /// This initializes all internal message collections
        /// </summary>
        public WebMessagesModel()
        {
            this.InfoMessages = new List<WebMessage>();
            this.SuccessMessages = new List<WebMessage>();
            this.ErrorMessages = new List<WebMessage>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebMessagesModel"/> class.
        /// This initializes internal message lists with appropriate messages from common list (retrieved from Session)
        /// </summary>
        /// <param name="messages">The messages list, containing all types of messages.</param>
        public WebMessagesModel(List<WebMessage> messages)
        {
            this.InfoMessages = messages.Where(msg => msg.WebMessageType == WebMessageType.Informative).ToList();
            this.ErrorMessages = messages.Where(msg => msg.WebMessageType == WebMessageType.Error).ToList();
            this.SuccessMessages = messages.Where(msg => msg.WebMessageType == WebMessageType.Success).ToList();
        }

        /// <summary>
        /// List of informative messages
        /// </summary>
        public List<WebMessage> InfoMessages { get; set; }

        /// <summary>
        /// List of success messages
        /// </summary>
        public List<WebMessage> SuccessMessages { get; set; }

        /// <summary>
        /// List of error messages
        /// </summary>
        public List<WebMessage> ErrorMessages { get; set; }

        /// <summary>
        /// Gets a value indicating whether is there any message in any of message collections.
        /// </summary>
        public bool IsAnyMessage
        {
            get
            {
                return this.InfoMessages.Count + this.ErrorMessages.Count + this.SuccessMessages.Count > 0;
            }
        }
    }
}