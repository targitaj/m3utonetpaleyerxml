namespace Uma.Eservices.DbObjects
{
    using System.Diagnostics;

    /// <summary>
    /// This is "VIEW: Read-only" POCO class to retrieve all model translations
    /// and passing them to business logic.
    /// Note: There is no corresponding table in DB, this is kinda filling from View/SP
    /// </summary>
    [DebuggerDisplay("Name:{PropertyName} Type:{TranslationType} Lang:{Language}")]
    public class ModelTranslation
    {
        /// <summary>
        /// Gets or sets the PropertyName property should be part of model
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Type of translated text - is it prompt, help text or control text (placeholder)
        /// </summary>
        public TranslatedTextType TranslationType { get; set; }

        /// <summary>
        /// Language of translated text
        /// </summary>
        public SupportedLanguage Language { get; set; }

        /// <summary>
        /// Actiual translation itself
        /// </summary>
        public string TranslatedText { get; set; }
    }
}
