namespace Uma.Eservices.DbObjects
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Database object to store Help/Support FAQ Answers/Questions
    /// </summary>
    [DebuggerDisplay("Id: {Id} FaqTransCount: {FaqTranslations.Count())")]
    public class Faq
    {
        /// <summary>
        /// Gets or sets the FAQ id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the FAQ short description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the related FaqTranslations collection
        /// </summary>
        public virtual ICollection<FaqTranslation> FaqTranslations { get; set; }
    }
}
