namespace Uma.Eservices.Logic
{
    using System;
    using FluentValidation.Resources;

    /// <summary>
    /// Additional implementation of FluentValidation Message string source to store original text
    /// </summary>
    public class DatabaseStringSource : IStringSource
    {
        /// <summary>
        /// Message storage
        /// </summary>
        private readonly string message;

        /// <summary>
        /// Creates a new StringErrorMessageSource using the specified error message as the error template.
        /// </summary>
        /// <param name="message">The error message translation.</param>
        /// <param name="originalMessage">The original message.</param>
        public DatabaseStringSource(string message, string originalMessage)
        {
            this.message = message;
            this.ResourceName = originalMessage;
        }

        /// <summary>
        /// Construct the error message template
        /// </summary>
        /// <returns>Error message template</returns>
        public string GetString()
        {
            return this.message;
        }

        /// <summary>
        /// The name of the resource if localized.
        /// </summary>
        public string ResourceName
        {
            get;
            set;
        }

        /// <summary>
        /// The type of the resource provider if localized.
        /// </summary>
        public Type ResourceType
        {
            get { return null; }
        }
    }
}