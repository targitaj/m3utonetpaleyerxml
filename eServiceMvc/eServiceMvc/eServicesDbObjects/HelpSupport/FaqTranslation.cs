namespace Uma.Eservices.DbObjects
{
    using System.Diagnostics;

    /// <summary>
    /// Database object for Help/Support page FAQ Answer/Question translation
    /// </summary>
    [DebuggerDisplay("Id: {Id} Lang: {Language.ToString()}")]
    public class FaqTranslation
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
