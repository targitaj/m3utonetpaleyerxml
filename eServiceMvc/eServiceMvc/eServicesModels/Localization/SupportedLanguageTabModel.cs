namespace Uma.Eservices.Models.Localization
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// This model is used to provide tabs in localization View
    /// </summary>
    public class SupportedLanguageTabModel
    {
        /// <summary>
        /// Defines supported languages
        /// </summary>
        public List<LanguageTabModel> LanguageTabList { get; set; }

        /// <summary>
        /// Selected language item
        /// </summary>
        public LanguageTabModel SelectedLanguage { get; set; }
    }
}
