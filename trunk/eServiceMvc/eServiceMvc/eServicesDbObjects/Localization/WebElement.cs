namespace Uma.Eservices.DbObjects
{
    using System.Collections.Generic;

    /// <summary>
    /// Db object used to manipulate with Resources
    /// </summary>
    public class WebElement
    {
        /// <summary>
        /// Gets or sets the ResourceID
        /// </summary>
        public int WebElementId { get; set; }

        /// <summary>
        /// Gets or sets the ModelName model name must be same as ViewModel name
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Gets or sets the PropertyName property should be part of model
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the related WebElement label, sublabel, help and placeholder translations collection
        /// </summary>
        public virtual ICollection<WebElementTranslation> WebElementTranslations { get; set; }

        /// <summary>
        /// Gets or sets the related WebElement Validation message translations collection
        /// </summary>
        public virtual ICollection<WebElementValidationTranslation> WebElementValidationTranslations { get; set; }
    }
}
