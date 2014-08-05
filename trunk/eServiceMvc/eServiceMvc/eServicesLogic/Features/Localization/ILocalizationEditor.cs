namespace Uma.Eservices.Logic.Features.Localization
{
    using Uma.Eservices.Models.Localization;

    /// <summary>
    /// Used to GET/ POST / UPDATE WeElements
    /// </summary>
    public interface ILocalizationEditor
    {
        /// <summary>
        /// Method save WebElementModel object. If this is new model then new WebElement with childrens will be created
        /// If TranslatedText property will be empty it it will be deleted from DB. If TranslatedText is not empty and have ID then updated.
        /// After entity updates changes will be forced to update entity
        /// </summary>
        /// <param name="model">WebElementModel object model</param>
        void SaveResources(WebElementModel model);

        /// <summary>
        /// This method returns Web.Resource moel filtered by provided input values
        /// Returns null if Resource is not found in db.
        /// </summary>
        /// <param name="propertyName">Property Name</param>
        /// <param name="modelName">Model name</param>
        /// <param name="selectedLang">Selected Language enum type, default English. Define this value to filter specific values by language</param>
        /// <returns>Web.Resource object or null if resource is not found in db</returns>
        WebElementModel GetResource(string propertyName, string modelName, SupportedLanguage selectedLang = SupportedLanguage.English);

        /// <summary>
        /// Update or create original text in DB
        /// </summary>
        /// <param name="model">Contains information about translation</param>
        void AddUpdateOriginalText(TranslatePageModel model);

        /// <summary>
        /// Method gets translated string from DB.
        /// </summary>
        /// <param name="text">Text to be translated</param>
        /// <param name="featureName">Model name</param>
        /// <param name="selectedLanguage">Language wich is selcted</param>
        /// <returns>Translated string from db or default value</returns>
        TranslatePageModel GetTranslatePageModel(string text, string featureName, SupportedLanguage selectedLanguage = SupportedLanguage.English);
    }
}
