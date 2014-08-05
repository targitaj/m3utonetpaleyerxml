namespace Uma.Eservices.Logic.Features.Localization
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Uma.Eservices.Common;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;
    using Web = Uma.Eservices.Models.Localization;

    /// <summary>
    /// ILocalizationManager inteface implementation. This class is implemented without caching.
    /// Used to retrieve and display text and control label translations on UI
    /// </summary>
    public class LocalizationManager : ILocalizationManager
    {
        /// <summary>
        /// Gets ILocalizationDataHelper manager type
        /// </summary>
        private ILocalizationDataHelper LocalDbContext { get; set; }

        /// <summary>
        /// Gets the feature name from namespace.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <exception cref="EserviceException">
        /// There is a resource residing outside "Features" namespace.
        /// or
        /// There is a resource missing its Feature name under "Features" namespace.
        /// </exception>
        public static string GetFeatureNameFromNamespace(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentNullException("resourceName");
            }
            string featureName;

            if (resourceName == "CaptchaMvc.Controllers")
            {
                featureName = "CaptchaMvc";
            }
            else
            {
                string[] namespaceParts = resourceName.Split('.');
                int index = Array.IndexOf(namespaceParts, "Features") + 1;
                if (index == 0)
                {
                    throw new EserviceException("There should be no resource residing outside \"Features\" namespace.");
                }

                if (index >= namespaceParts.Length)
                {
                    throw new EserviceException(
                        "There is a resource missing its Feature name under \"Features\" namespace.");
                }

                featureName = namespaceParts[index];
            }

            return featureName;
        }

        /// <summary>
        /// Class constructor. Parameter is resolved from Unity configuration file
        /// </summary>
        /// <param name="localDbContext">Instance of ILocalizationDataHelper</param>
        public LocalizationManager(ILocalizationDataHelper localDbContext)
        {
            this.LocalDbContext = localDbContext;
        }

        /// <summary>
        /// Gets List of WebElement translations. Converts to WebTranslation (Web-model) object
        /// throws ArgumentNullException if passed value is null or empty
        /// </summary>
        /// <param name="modelName">Model name</param>
        /// <returns>WebElementLocalizer object</returns>
        public WebElementLocalizer GetWebElementTranslations(string modelName)
        {
            if (string.IsNullOrEmpty(modelName))
            {
                throw new ArgumentNullException("modelName");
            }

            return new WebElementLocalizer(this.LocalDbContext.GetAllModelTranslations(modelName).ToWebModel());
        }

        /// <summary>
        /// Method gets translated string from DB, used for production.
        /// </summary>
        /// <param name="text">Text to be translated</param>
        /// <param name="featureName">Model name</param>
        /// <param name="args">The arguments of a text.</param>
        /// <returns>Translated string from db or default value</returns>
        public string GetTextTranslationPROD(string text, string featureName, params object[] args)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            if (string.IsNullOrEmpty(featureName))
            {
                throw new ArgumentNullException("featureName");
            }

            var language = Globalizer.CurrentUICultureLanguage.Value;
            var translation = this.GetTextTranslation(text, featureName, language);

            if (translation == null && language != Web.SupportedLanguage.English)
            {
                translation = this.GetTextTranslation(text, featureName, Web.SupportedLanguage.English);
            }

            return translation ?? text;
        }

        /// <summary>
        /// Method gets translated string from DB, used for testing.
        /// </summary>
        /// <param name="text">Text to be translated</param>
        /// <param name="featureName">Model name</param>
        /// <param name="args">The arguments of a text.</param>
        /// <returns>Translated string from db or default value</returns>
        public string GetTextTranslationTEST(string text, string featureName, params object[] args)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (string.IsNullOrEmpty(featureName))
            {
                throw new ArgumentNullException("featureName");
            }

            var language = Globalizer.CurrentUICultureLanguage.Value;
            var translation = this.GetTextTranslation(text, featureName, language);

            string res = null;

            if (!string.IsNullOrEmpty(translation))
            {
                res = translation;
            }
            else
            {
                if (language != Web.SupportedLanguage.English)
                {
                    translation = this.GetTextTranslation(text, featureName, Web.SupportedLanguage.English);
                    res = string.IsNullOrEmpty(translation) ? string.Concat("T:", text) : string.Concat(Globalizer.CurrentUICultureLanguage.Key, ":", translation);
                }
            }

            return string.Format(CultureInfo.CurrentUICulture, res ?? "T:" + text, args);
        }

        /// <summary>
        /// Method gets translated string from DB.
        /// </summary>
        /// <param name="text">Text to be translated</param>
        /// <param name="featureName">Model name</param>
        /// <param name="language">Language for translation</param>
        /// <returns>Translated string from db or default value</returns>
        private string GetTextTranslation(string text, string featureName, Web.SupportedLanguage language)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            if (string.IsNullOrEmpty(featureName))
            {
                throw new ArgumentNullException("featureName");
            }

            string returnValue = null;
            var origText = this.LocalDbContext.Get<OriginalText>(o => o.Original == text && o.Feature == featureName);

            if (origText == null)
            {
                return null;
            }

            var tran = origText.OriginalTextTranslations.FirstOrDefault(o => o.Language == language.ToDbObject());
            if (tran != null)
            {
                returnValue = tran.Translation;
            }

            return returnValue;
        }

        /// <summary>
        /// Text formating for translator to have ability to navigate to translate page
        /// </summary>
        /// <param name="text">Text to be translated</param>
        /// <param name="translationUrl">Url to translation page</param>
        public string FormatTranslationForTranslator(string text, string translationUrl)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            if (translationUrl == null)
            {
                throw new ArgumentNullException("translationUrl");
            }

            return string.Format(
                CultureInfo.CurrentUICulture,
                @"<span href='{1}' class=""EditorLink""><i class=""fa fa-pencil""></i></span> {0}",
                text,
                translationUrl);
        }

        /// <summary>
        /// Retrieves translated validation (error) message for specified model/property and original message variation
        /// </summary>
        /// <param name="originalMessage">Original validation message as hardcoded in development time</param>
        /// <param name="modelName">Model name of validatable message</param>
        /// <param name="propertyName">Property name in model for which message is added</param>
        /// <returns>
        /// If found - translation, otherwise - prepended with "V:" original message
        /// </returns>
        public string GetValidatorTranslationTEST(string originalMessage, string modelName, string propertyName)
        {
            if (string.IsNullOrEmpty(originalMessage))
            {
                throw new ArgumentNullException("originalMessage");
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            var language = Globalizer.CurrentUICultureLanguage.Value;
            var translation = this.GetValidationTranslation(originalMessage, modelName, propertyName, language);

            string res = null;

            if (!string.IsNullOrEmpty(translation))
            {
                res = translation;
            }
            else
            {
                if (language != Web.SupportedLanguage.English)
                {
                    translation = this.GetValidationTranslation(originalMessage, modelName, propertyName, Web.SupportedLanguage.English);
                    res = string.IsNullOrEmpty(translation) ? string.Concat("V:", originalMessage) : string.Concat(Globalizer.CurrentUICultureLanguage.Key, ":", translation);
                }
            }

            return res ?? "V:" + originalMessage;
        }

        /// <summary>
        /// Retrieves translated validation (error) message for specified model/property and original message variation
        /// </summary>
        /// <param name="originalMessage">Original validation message as hardcoded in development time</param>
        /// <param name="modelName">Model name of validatable message</param>
        /// <param name="propertyName">Property name in model for which message is added</param>
        /// <returns>
        /// If found - translation, otherwise - original message
        /// </returns>
        public string GetValidatorTranslationPROD(string originalMessage, string modelName, string propertyName)
        {
            if (string.IsNullOrEmpty(originalMessage))
            {
                throw new ArgumentNullException("originalMessage");
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            var language = Globalizer.CurrentUICultureLanguage.Value;
            var translation = this.GetValidationTranslation(originalMessage, modelName, propertyName, language);

            string res = null;

            if (!string.IsNullOrEmpty(translation))
            {
                res = translation;
            }
            else
            {
                if (language != Web.SupportedLanguage.English)
                {
                    translation = this.GetValidationTranslation(originalMessage, modelName, propertyName, Web.SupportedLanguage.English);
                    res = string.IsNullOrEmpty(translation) ? null : translation;
                }
            }

            return res ?? originalMessage;
        }

        /// <summary>
        /// Method gets translated string from DB.
        /// </summary>
        /// <param name="originalMessage">Text to be translated</param>
        /// <param name="modelName">Model name</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="language">Language for translation</param>
        /// <returns>Translated string from db or null</returns>
        private string GetValidationTranslation(string originalMessage, string modelName, string propertyName, Web.SupportedLanguage language)
        {
            string returnValue = null;
            var modelProperty = this.LocalDbContext.Get<WebElement>(o => o.ModelName == modelName && o.PropertyName == propertyName);

            if (modelProperty == null)
            {
                return null;
            }

            var tran = modelProperty.WebElementValidationTranslations.FirstOrDefault(o => o.Language == language.ToDbObject());
            if (tran != null)
            {
                returnValue = tran.TranslatedValidation;
            }

            return returnValue;
        }
    }
}
