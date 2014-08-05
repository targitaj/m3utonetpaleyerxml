namespace Uma.Eservices.DbObjects
{
    using System;

    /// <summary>
    /// Translation for original text, that contains in OriginalText table
    /// </summary>
    public class OriginalTextTranslation : IBaseDataObject<Guid>
    {
        /// <summary>
        /// Gets or sets the identifier of a OriginalTextTranslation.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Reference value (foreign key) for linking <see cref="OriginalText"/> with this translation
        /// </summary>
        public Guid OriginalTextId { get; set; }

        /// <summary>
        /// Reference value (foreign key) for linking <see cref="OriginalText"/> with this translation
        /// </summary>
        public OriginalText OriginalText { get; set; }

        /// <summary>
        /// Gets or sets language for curent translation
        /// </summary>
        public SupportedLanguage Language { get; set; }

        /// <summary>
        /// Gets or sets translation text
        /// </summary>
        public string Translation { get; set; }
    }
}
