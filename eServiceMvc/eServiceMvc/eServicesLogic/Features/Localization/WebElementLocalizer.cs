namespace Uma.Eservices.Logic.Features.Localization
{
    using System.Collections.Generic;
    using Uma.Eservices.Models.Localization;
    using db = Uma.Eservices.DbObjects;

    /// <summary>
    /// Class, which instance to use in HTML Helpers for translations to their "static texts"
    /// </summary>
    public class WebElementLocalizer
    {
        /// <summary>
        /// Readonly WebElementTranslationModel List field that holds text translation items 
        /// </summary>
        private readonly List<WebElementTranslationModel> modelElementTranslations;

        /// <summary>
        /// WebElementLocalizer ctor. Sets translations List
        /// </summary>
        /// <param name="modelTranslations">List of type WebElementTranslationModel</param>
        public WebElementLocalizer(List<WebElementTranslationModel> modelTranslations)
        {
            if (modelTranslations == null)
            {
                this.modelElementTranslations = new List<WebElementTranslationModel>();
                return;
            }

            this.modelElementTranslations = modelTranslations;
        }

        /// <summary>
        /// Gets the translation of required property in CurrentUiCulture language for requested text type.
        /// Note: Returns empty string if translation is not found both in requested language and in fallback (default) language
        /// </summary>
        /// <param name="propertyName">Name of the property (in view model, which needs translation).</param>
        /// <param name="textType">Type of the text in web element (out of "Propmpt", "HelpText" and "Placeholder/Default value" etc.).</param>
        /// <returns>Translation (if found) or empty string if not found.</returns>
        public string GetTranslation(string propertyName, TranslatedTextType textType)
        {
            SupportedLanguage currentLanguage = Globalizer.CurrentUICultureLanguage.Value;
            return this.ProductionBehavior(propertyName, currentLanguage, textType);
        }

        /// <summary>NOTE:
        /// ====== THIS METHOD MUST BE USED ONLY FOR TESTING PURPOSES! ======
        /// 
        /// Gets the translation of required property in CurrentUiCulture language for requested text type.
        /// and this Testing version will return "R:PropertyName" for missing translations (not desirable behavior in production)
        /// Use only within controlled compile time blocks (#if !PROD)
        /// Secondly - we can have both test-time and runtime behavioral unit-tests
        /// </summary>
        /// <param name="propertyName">Name of the property (in view model, which needs translation).</param>
        /// <param name="textType">Type of the text in web element (out of "Propmpt", "HelpText" and "Placeholder/Default value" etc.).</param>
        /// <param name="enumPropertyName">Name of the property in enum</param>
        /// <returns>Translation (if found) or "R:PropertyName" for missing translations</returns>
        public string GetTestableTranslation(string propertyName, TranslatedTextType textType, string enumPropertyName = null)
        {
            SupportedLanguage currentLanguage = Globalizer.CurrentUICultureLanguage.Value;
            return this.TestingBehavior(propertyName, currentLanguage, textType, enumPropertyName);
        }

        /// <summary>
        /// Production behavior is as follows:
        /// 1) Locate CurrentCulture Language translation, if found - return it
        /// 2) If not found - try default language (0 = English), if this found - return it
        /// 3) If not found at all (or list is empty) - return empty string
        /// </summary>
        /// <param name="propertyName">Name of the property (in view model, which needs translation).</param>
        /// <param name="language">Value of the language in SupportedLanguages Enum.</param>
        /// <param name="textType">Type of the text in web element (out of "Propmpt", "HelpText" and "Placeholder/Default value" etc.) index in defined ENUM.</param>
        private string ProductionBehavior(string propertyName, SupportedLanguage language, TranslatedTextType textType)
        {
            string translation = this.LocateTranslation(propertyName, language, textType);

            // if not found in current language and it is not default already - try default language ("0" value in Globalizer implemented Language list)
            if (translation == null && language != Globalizer.ImplementedCultures[0].Value)
            {
                translation = this.LocateTranslation(propertyName, Globalizer.ImplementedCultures[0].Value, textType);
            }

            return translation ?? string.Empty;
        }

        /// <summary>
        /// Testings behavior as follows:
        /// 1) If found in requested language - returns it as-is
        /// 2) Checks fallback/default language - if found - prepends original language code to it and returns, like "fi:This is fallback translation"
        /// 3) Translation does not exist at all - returns property name prepended with R, like "R:PropertyName"
        /// </summary>
        /// <param name="propertyName">Name of the property (in view model, which needs translation).</param>
        /// <param name="language">Value of the language in SupportedLanguages Enum.</param>
        /// <param name="textType">Value of the text in web element (out of "Propmpt", "HelpText" and "Placeholder/Default value" etc.) index in defined ENUM.</param>
        /// <param name="enumPropertyName">Name of the property in enum</param>
        /// <returns>Translated Text</returns>
        private string TestingBehavior(string propertyName, SupportedLanguage language, TranslatedTextType textType, string enumPropertyName = null)
        {
            string translation = this.LocateTranslation(propertyName, language, textType);

            if (!string.IsNullOrEmpty(translation))
            {
                return translation;
            }

            // if not found in current language and it is not default already - try default language ("0" value in Supported Languages list)
            if (translation == null && language != SupportedLanguage.English)
            {
                translation = this.LocateTranslation(propertyName, SupportedLanguage.English, textType);

                if (translation == null && textType == TranslatedTextType.Label)
                {
                    return string.Concat("R:", propertyName);
                }

                if (textType == TranslatedTextType.Label)
                {
                    var initialLanguageName = Globalizer.CurrentUICultureLanguage.Key;
                    return string.Concat(initialLanguageName, ":", translation);
                }
            }

            if (textType == TranslatedTextType.Label || textType == TranslatedTextType.EnumText)
            {
                return string.Concat("R:", propertyName);
            }

            return string.Empty;
        }

        /// <summary>
        /// Locates the translation in internal (cache) list and returns. 
        /// If not found will return NULL (to ease check)
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="language">Value of the SupportedLanguage.</param>
        /// <param name="textType">Value of the TranslatedTextType</param>
        /// <returns>Found translation string or NULL if not found</returns>
        private string LocateTranslation(string propertyName, SupportedLanguage language, TranslatedTextType textType)
        {
            // FOR: because it is fastest locator http://www.schnieds.com/2009/03/linq-vs-foreach-vs-for-loop-performance.html
            for (int i = 0; i < this.modelElementTranslations.Count; i++)
            {
                if (this.modelElementTranslations[i].PropertyName == propertyName
                    && this.modelElementTranslations[i].Language == language
                    && this.modelElementTranslations[i].TranslationType == textType)
                {
                    return this.modelElementTranslations[i].TranslatedText;
                }
            }

            // not found if came here
            return null;
        }
    }
}