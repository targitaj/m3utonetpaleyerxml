namespace Uma.Eservices.Models.Localization
{
    using System.Diagnostics;

    /// <summary>
    /// Langugage tab model specifies model for available languages, selected language.
    /// Used to bind view controls for language tabs
    /// </summary>
    [DebuggerDisplay("Language: {Language.ToString()}")]
    public class LanguageTabModel
    {
        /// <summary>
        /// Gets or sets Language type from Model  to View and vice versa
        /// </summary>
        public SupportedLanguage Language { get; set; }

        /// <summary>
        /// Get or Sets selected language
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
