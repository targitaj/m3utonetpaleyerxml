namespace Uma.Eservices.Models.Localization
{
    using System;

    using Uma.Eservices.Models.Localization;

    /// <summary>
    /// Used to manipulate with OriginalText
    /// </summary>
    public class TranslatePageTranslationModel
    {
        /// <summary>
        /// Gets or sets the Language
        /// </summary>
        public SupportedLanguage Language { get; set; }

        /// <summary>
        /// Gets or sets the TranslatedText. This is acutal text translation
        /// </summary>
        public string TranslatedText { get; set; }
    }
}
