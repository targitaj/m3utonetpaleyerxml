namespace Uma.Eservices.DbObjects
{
    /// <summary>
    /// ResourceTranslation object is used to get text translations.
    /// This class also contains translation text type, and language enums
    /// </summary>
    public class WebElementValidationTranslation
    {
        /// <summary>
        /// Gets or sets the TranslationId
        /// </summary>
        public int TranslationId { get; set; }

        /// <summary>
        /// Gets or sets the ResourceID this id is related to Resource  obj
        /// </summary>
        public int WebElementId { get; set; }

        /// <summary>
        /// Gets or sets the Language
        /// </summary>
        public SupportedLanguage Language { get; set; }

        /// <summary>
        /// Gets or sets the TranslatedText. This is acutal text translation
        /// </summary>
        public string TranslatedValidation { get; set; }
    }
}
