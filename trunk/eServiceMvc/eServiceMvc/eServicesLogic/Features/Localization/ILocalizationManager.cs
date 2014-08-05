namespace Uma.Eservices.Logic.Features.Localization
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Uma.Eservices.Models.Localization;
    using Uma.Eservices.Models.Sandbox;

    /// <summary>
    /// ILocalizationManager interface used for manipulate with resources from DB (CRUD operations)
    /// </summary>
    public interface ILocalizationManager
    {
        /// <summary>
        /// Gets List of WebElement translations. Converts to WebTranslation (Web-model) object
        /// throws ArgumentNullException if passed value is null or empty
        /// </summary>
        /// <param name="modelName">Model name</param>
        /// <returns>WebElementLocalizer object</returns>
        WebElementLocalizer GetWebElementTranslations(string modelName);

        /// <summary>
        /// Method gets translated string from DB, used for testing.
        /// </summary>
        /// <param name="text">Text to be translated</param>
        /// <param name="featureName">Model name</param>
        /// <param name="args">The arguments of a text.</param>
        /// <returns>Translated string from db or default value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "TEST", Justification = "Used to define test version")]
        string GetTextTranslationTEST(
            string text,
            string featureName,
            params object[] args);

        /// <summary>
        /// Method gets translated string from DB, used for production.
        /// </summary>
        /// <param name="text">Text to be translated</param>
        /// <param name="featureName">Model name</param>
        /// <param name="args">The arguments of a text.</param>
        /// <returns>Translated string from db or default value</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "PROD", Justification = "Used to define prodution version")]
        string GetTextTranslationPROD(
            string text,
            string featureName,
            params object[] args);

        /// <summary>
        /// Text formating for translator to have ability to navigate to translate page
        /// </summary>
        /// <param name="text">Text to be translated</param>
        /// <param name="translationUrl">Url to translation page</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "need string", Justification = "No need for Uri object usage")]
        string FormatTranslationForTranslator(string text, string translationUrl);

        /// <summary>
        /// Retrieves translated validation (error) message for specified model/property and original message variation
        /// </summary>
        /// <param name="originalMessage">Original validation message as hardcoded in development time</param>
        /// <param name="modelName">Model name of validatable message</param>
        /// <param name="propertyName">Property name in model for which message is added</param>
        /// <returns>If found - translation, otherwise - prepended with "V:" original message</returns>
        string GetValidatorTranslationTEST(string originalMessage, string modelName, string propertyName);

        /// <summary>
        /// Retrieves translated validation (error) message for specified model/property and original message variation
        /// </summary>
        /// <param name="originalMessage">Original validation message as hardcoded in development time</param>
        /// <param name="modelName">Model name of validatable message</param>
        /// <param name="propertyName">Property name in model for which message is added</param>
        /// <returns>If found - translation, otherwise - original message</returns>
        string GetValidatorTranslationPROD(string originalMessage, string modelName, string propertyName);
    }
}
