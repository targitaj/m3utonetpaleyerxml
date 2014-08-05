namespace Uma.Eservices.Models.HelpSupport
{
    using Uma.Eservices.Models.Localization;

    /// <summary>
    /// View model for Help page FAQ item
    /// </summary>
    public class FaqTranslationModel
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
        /// Gets or sets the Language
        /// </summary>
        public SupportedLanguage Language { get; set; }
    }
}
