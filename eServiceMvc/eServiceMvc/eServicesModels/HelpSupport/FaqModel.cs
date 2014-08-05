namespace Uma.Eservices.Models.HelpSupport
{
    using System.Collections.Generic;

    /// <summary>
    /// View model for Help page FAQ items
    /// </summary>
    public class FaqModel
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
        public List<FaqTranslationModel> FaqTranslations { get; set; }
    }
}
