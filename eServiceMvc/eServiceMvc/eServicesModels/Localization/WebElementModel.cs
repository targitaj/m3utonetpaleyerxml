namespace Uma.Eservices.Models.Localization
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Db object used to manipulate with Resources
    /// </summary>
    [DebuggerDisplay("Id: {ResourceId} / propName: {PropertyName} / modelName: {ModelName}")]
    public class WebElementModel
    {
        /// <summary>
        /// Gets or sets the ResourceID
        /// </summary>
        public int WebElementId { get; set; }

        /// <summary>
        /// Gets or sets the ModelName
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Gets or sets the PropertyName property should be part of model
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the Language which is used for language selector - shows current loaded language
        /// </summary>
        public SupportedLanguage SelectedLanguage { get; set; }

        /// <summary>
        /// Gets or sets the Language which should be used to save edited values
        /// </summary>
        public SupportedLanguage LanguageToSave { get; set; }

        /// <summary>
        /// Text which is used for control Label
        /// </summary>
        public string PropertyLabel { get; set; }

        /// <summary>
        /// Text which is used for property sub-Label
        /// </summary>
        public string PropertySubLabel { get; set; }

        /// <summary>
        /// Text to use as hint (placeholder) inside field
        /// </summary>
        public string PropertyHint { get; set; }

        /// <summary>
        /// Property extended help text - to use within question-mark help popup
        /// </summary>
        public string PropertyHelp { get; set; }

        /// <summary>
        /// Enum element (item) text -> used fro ratio buttons and enum dropdowns
        /// </summary>
        public string PropertyEnum { get; set; }

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
