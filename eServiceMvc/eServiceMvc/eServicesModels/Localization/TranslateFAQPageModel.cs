namespace Uma.Eservices.Models.Localization
{
    using System.Collections.Generic;

    /// <summary>
    /// Used to manipulate with FAQ model
    /// </summary>
    public class TranslateFAQPageModel
    {
        /// <summary>
        /// Gets or sets the FaqTranslations Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Parent FAQ id
        /// </summary>
        public int FaqId { get; set; }

        /// <summary>
        /// Gets or sets the Question
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets the Answer
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Gets or sets the Language which is used for language selector - shows current loaded language
        /// </summary>
        public SupportedLanguage SelectedLanguage { get; set; }

        /// <summary>
        /// Gets or sets the Language to save
        /// </summary>
        public SupportedLanguage LanguageToSave { get; set; }

        /// <summary>
        /// Get or sets ReturnLink that is used if user click save and return button
        /// </summary>
        public string ReturnLink { get; set; }

        /// <summary>
        /// If True - submit will return to initial page, otherwise - reloads translator (with changed language)
        /// </summary>
        public bool IsReturnBack { get; set; }
    }
}