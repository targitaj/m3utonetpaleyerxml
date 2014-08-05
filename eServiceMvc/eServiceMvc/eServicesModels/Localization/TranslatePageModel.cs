namespace Uma.Eservices.Models.Localization
{
    /// <summary>
    /// Used to manipulate translations with OriginalText, used in T() method
    /// </summary>
    public class TranslatePageModel
    {
        /// <summary>
        /// Gets or sets Feature
        /// </summary>
        public string Feature { get; set; }

        /// <summary>
        /// Gets or sets translated Text
        /// </summary>
        public string OriginalText { get; set; }

        /// <summary>
        /// Gets or sets Language which should be used when saving data from form
        /// </summary>
        public SupportedLanguage LanguageToSave { get; set; }

        /// <summary>
        /// Gets or sets Language which has been selected in form (to be loaded as language change reload)
        /// </summary>
        public SupportedLanguage SelectedLanguage { get; set; }

        /// <summary>
        /// Text that is adjusted for <see cref="SelectedLanguage"/>
        /// </summary>
        public string TranslatedText { get; set; }

        /// <summary>
        /// Get or sets ReturnLink that is used if user click save and return button
        /// </summary>
        public string ReturnLink { get; set; }

        /// <summary>
        /// Property to control form submit behavior.
        /// When False (Default) - reloads translation page
        /// When True - returns back to page where initial call was made
        /// </summary>
        public bool IsReturnBack { get; set; }
    }
}