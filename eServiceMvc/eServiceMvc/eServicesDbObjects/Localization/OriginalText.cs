namespace Uma.Eservices.DbObjects
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Text that will be translated
    /// </summary>
    public class OriginalText : IBaseDataObject<Guid>
    {
        /// <summary>
        /// Store collection of <see cref="OriginalTextTranslation"/>
        /// </summary>
        private ICollection<OriginalTextTranslation> originalTextTranslations;

        /// <summary>
        /// Gets or sets the identifier of a OriginalText.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the Feature name it's *.cshtm files
        /// </summary>
        public string Feature { get; set; }

        /// <summary>
        /// Gets or sets the original text that will be translated
        /// </summary>
        public string Original { get; set; }

        /// <summary>
        /// Gets or sets collection of <see cref="OriginalTextTranslation"/>
        /// </summary>
        public virtual ICollection<OriginalTextTranslation> OriginalTextTranslations
        {
            get
            {
                return this.originalTextTranslations ?? (this.originalTextTranslations = new List<OriginalTextTranslation>());
            }

            set { this.originalTextTranslations = value; }
        }
    }
}
