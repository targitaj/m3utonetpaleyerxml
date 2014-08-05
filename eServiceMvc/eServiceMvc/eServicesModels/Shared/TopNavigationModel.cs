namespace Uma.Eservices.Models.Shared
{
    /// <summary>
    /// Model for Top navigation part of site
    /// </summary>
    public class TopNavigationModel
    {
        /// <summary>
        /// Contains UPPERCASE current UI language code.
        /// Can be three possibilities: EN, FI and SV
        /// </summary>
        public string CurrentLanguage { get; set; }

        /// <summary>
        /// Controls the way of disabling language selector in header
        /// </summary>
        public bool LanguagesDisabled { get; set; }

        /// <summary>
        /// Provides a flag to disable Authentication Link(s) in page
        /// </summary>
        public bool AuthenticationDisabled { get; set; }

        /// <summary>
        /// Applies special class when EN is selected language
        /// </summary>
        public string EnglishSelectorStyle 
        {
            get
            {
                return this.CurrentLanguage == "EN" ? "class='sel-lang'" : string.Empty;
            }
        }

        /// <summary>
        /// Applies special class when FI is selected language
        /// </summary>
        public string FinnishSelectorStyle
        {
            get
            {
                return this.CurrentLanguage == "FI" ? "class='sel-lang'" : string.Empty;
            }
        }

        /// <summary>
        /// Applies special class when SE is selected language
        /// </summary>
        public string SwedishSelectorStyle
        {
            get
            {
                return this.CurrentLanguage == "SV" ? "class='sel-lang'" : string.Empty;
            }
        }
    }
}
