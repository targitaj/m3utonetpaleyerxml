namespace Uma.Eservices.Logic.Features.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Uma.Eservices.Common;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;
    using Web = Uma.Eservices.Models.Localization;

    /// <summary>
    /// ILocalizationEditor interface implementation. Used to GET/ POST / UPDATE WeElements and Text block translations
    /// This implementation don't use Caching
    /// </summary>
    public class LocalizationEditor : ILocalizationEditor
    {
        /// <summary>
        /// Gets beasic Db operation querys.
        /// </summary>
        private IGeneralDataHelper DbContext { get; set; }

        /// <summary>
        /// Class constructor. Parameter is resolved from Unity configuration file
        /// </summary>
        /// <param name="dbContext">Instance of IGeneralDataHelper</param>
        public LocalizationEditor(IGeneralDataHelper dbContext)
        {
            this.DbContext = dbContext;
        }

        /// <summary>
        /// Method gets translation model got "T" text block from DB
        /// </summary>
        /// <param name="text">Text to be translated</param>
        /// <param name="featureName">Feature name within Web site features folders</param>
        /// <param name="selectedLanguage">Language id (id must be in range of LanguageEnum)</param>
        /// <returns>Translated string from db or default value</returns>
        public Web.TranslatePageModel GetTranslatePageModel(string text, string featureName, Web.SupportedLanguage selectedLanguage = Web.SupportedLanguage.English)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            if (string.IsNullOrEmpty(featureName))
            {
                throw new ArgumentNullException("featureName");
            }

            var originalText = this.DbContext.Get<OriginalText>(o => o.Original == text && o.Feature == featureName)
                               ?? new OriginalText { Feature = featureName, Original = text };

            var webModel = originalText.ToWebModel();
            webModel.SelectedLanguage = selectedLanguage;
            webModel.LanguageToSave = selectedLanguage;
            webModel.IsReturnBack = true;

            if (originalText.OriginalTextTranslations == null)
            {
                return webModel;
            }

            var selectedLanguageTranslation = originalText.OriginalTextTranslations.FirstOrDefault(t => (int)t.Language == (int)selectedLanguage);
            webModel.TranslatedText = selectedLanguageTranslation != null ? selectedLanguageTranslation.Translation : string.Empty;
            return webModel;
        }

        /// <summary>
        /// Here comes POST from "T" text block translation form to save entered translation into database
        /// </summary>
        /// <param name="model">Contains information about translation</param>
        public void AddUpdateOriginalText(Web.TranslatePageModel model)
        {
            var originalText = this.DbContext.Get<OriginalText>(o => o.Original == model.OriginalText && o.Feature == model.Feature)
                ?? new OriginalText { Feature = model.Feature, Original = model.OriginalText };

            this.AddUpdateOriginalTextTranslation(originalText, model.LanguageToSave.ToDbObject(), model.TranslatedText);

            if (originalText.Id == Guid.Empty)
            {
                this.DbContext.Create(originalText);
            }
            else
            {
                this.DbContext.Update(originalText);
            }
        }

        /// <summary>
        /// This method returns Web.Resource model filled by provided input values
        /// Returns new empty model with defaults, if not found in database
        /// </summary>
        /// <param name="propertyName">Property Name</param>
        /// <param name="modelName">Model name</param>
        /// <param name="selectedLang">Selected Language enum type, default English. Define this value to filter specific values by language</param>
        public Web.WebElementModel GetResource(string propertyName, string modelName, Web.SupportedLanguage selectedLang = Web.SupportedLanguage.English)
        {
            if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(modelName))
            {
                throw new ArgumentNullException("propertyName", "propertyName or modelName is null or are empty for resource editing");
            }

            WebElement result = this.DbContext.Get<WebElement>(o => o.PropertyName == propertyName & o.ModelName == modelName)
                                ?? new WebElement { ModelName = modelName, PropertyName = propertyName };

            var webModel = result.ToWebModel(selectedLang);
            return webModel;
        }

        /// <summary>
        /// Method save WebElementModel object. If this is new model then new WebElement with childrens will be created
        /// If TranslatedText property will be empty it it will be deleted from DB. If TranslatedText is not empty then updated.
        /// After entity updates changes will be forced to update entity
        /// </summary>
        /// <param name="model">WebElementModel object model</param>
        public void SaveResources(Web.WebElementModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model", "model passed for saving is null");
            }

            if (model.WebElementId == 0)
            {
                // Creating new Web Element translation object, becaus no ID == no DB record
                var dbWebElement = new WebElement
                {
                    ModelName = model.ModelName,
                    PropertyName = model.PropertyName,
                    WebElementId = model.WebElementId,
                    WebElementTranslations = new List<WebElementTranslation>()
                };

                this.CreateWebObjectTranslations(model, dbWebElement);
                this.DbContext.Create<WebElement>(dbWebElement);
            }
            else
            {
                var dbWebElement = this.DbContext.Get<WebElement>(o => o.WebElementId == model.WebElementId);

                if (dbWebElement == null)
                {
                    throw new EserviceException("WebElement translation cannot be reloaded for edited data saving anymore");
                }

                this.AddOrUpdateWebObjectTranslation(dbWebElement, model, TranslatedTextType.Label);
                this.AddOrUpdateWebObjectTranslation(dbWebElement, model, TranslatedTextType.SubLabel);
                this.AddOrUpdateWebObjectTranslation(dbWebElement, model, TranslatedTextType.ControlText);
                this.AddOrUpdateWebObjectTranslation(dbWebElement, model, TranslatedTextType.HelpText);
                this.AddOrUpdateWebObjectTranslation(dbWebElement, model, TranslatedTextType.EnumText);

                this.DbContext.Update<WebElement>(dbWebElement);
            }
        }

        /// <summary>
        /// Update or create original text translation in DB
        /// </summary>
        /// <param name="originalText">Contains information about translation</param>
        /// <param name="language">Language for translation</param>
        /// <param name="translationText">Translation text</param>
        private void AddUpdateOriginalTextTranslation(OriginalText originalText, SupportedLanguage language, string translationText)
        {
            var originalTextTranslation = originalText.OriginalTextTranslations.FirstOrDefault(ott => ott.Language == language);

            if (!string.IsNullOrEmpty(translationText))
            {
                if (originalTextTranslation != null)
                {
                    originalTextTranslation.Translation = translationText;
                }
                else
                {
                    originalText.OriginalTextTranslations.Add(
                        new OriginalTextTranslation { Translation = translationText, Language = language });
                }
            }
            else
            {
                if (originalTextTranslation != null)
                {
                    this.DbContext.Delete(originalTextTranslation);
                }
            }
        }

        /// <summary>
        /// Method to add new translations to new Webelement object (when creating it)
        /// </summary>
        /// <param name="model">View model that contains translations in one language for particular WebElement</param>
        /// <param name="dbWebElement">Database object to be saved</param>
        private void CreateWebObjectTranslations(Web.WebElementModel model, WebElement dbWebElement)
        {
            if (!string.IsNullOrEmpty(model.PropertyLabel))
            {
                dbWebElement.WebElementTranslations.Add(
                    new WebElementTranslation { Language = model.LanguageToSave.ToDbObject(), TranslatedText = model.PropertyLabel, TranslationType = TranslatedTextType.Label });
            }

            if (!string.IsNullOrEmpty(model.PropertySubLabel))
            {
                dbWebElement.WebElementTranslations.Add(
                    new WebElementTranslation { Language = model.LanguageToSave.ToDbObject(), TranslatedText = model.PropertySubLabel, TranslationType = TranslatedTextType.SubLabel });
            }

            if (!string.IsNullOrEmpty(model.PropertyHint))
            {
                dbWebElement.WebElementTranslations.Add(
                    new WebElementTranslation { Language = model.LanguageToSave.ToDbObject(), TranslatedText = model.PropertyHint, TranslationType = TranslatedTextType.ControlText });
            }

            if (!string.IsNullOrEmpty(model.PropertyHelp))
            {
                dbWebElement.WebElementTranslations.Add(
                    new WebElementTranslation { Language = model.LanguageToSave.ToDbObject(), TranslatedText = model.PropertyHelp, TranslationType = TranslatedTextType.HelpText });
            }

            if (!string.IsNullOrEmpty(model.PropertyEnum))
            {
                dbWebElement.WebElementTranslations.Add(
                    new WebElementTranslation { Language = model.LanguageToSave.ToDbObject(), TranslatedText = model.PropertyEnum, TranslationType = TranslatedTextType.EnumText });
            }
        }

        /// <summary>
        /// Method that decides whether WebElement has specified type/langu
        /// </summary>
        /// <param name="elementToUpdate">Page translation and either updates/creates/deletes it depending on actual translation string</param>
        /// <param name="model">Model for translation updating</param>
        /// <param name="textType">Type of the text</param>
        private void AddOrUpdateWebObjectTranslation(WebElement elementToUpdate, Web.WebElementModel model, TranslatedTextType textType)
        {
            string valueToUpdate = string.Empty;
            switch (textType)
            {
                case TranslatedTextType.Label:
                    valueToUpdate = model.PropertyLabel;
                    break;
                case TranslatedTextType.SubLabel:
                    valueToUpdate = model.PropertySubLabel;
                    break;
                case TranslatedTextType.ControlText:
                    valueToUpdate = model.PropertyHint;
                    break;
                case TranslatedTextType.HelpText:
                    valueToUpdate = model.PropertyHelp;
                    break;
                case TranslatedTextType.EnumText:
                    valueToUpdate = model.PropertyEnum;
                    break;
            }

            var dbTranslation = elementToUpdate.WebElementTranslations.SingleOrDefault(
                                wt => wt.TranslationType == textType && wt.Language == model.LanguageToSave.ToDbObject());

            if (!string.IsNullOrEmpty(valueToUpdate))
            {
                if (dbTranslation == null)
                {
                    // add element
                    elementToUpdate.WebElementTranslations.Add(
                        new WebElementTranslation { Language = model.LanguageToSave.ToDbObject(), TranslatedText = valueToUpdate, TranslationType = textType });
                }
                else
                {
                    // update element
                    dbTranslation.TranslatedText = valueToUpdate;
                }
            }
            else
            {
                if (dbTranslation != null)
                {
                    // remove element
                    elementToUpdate.WebElementTranslations.Remove(dbTranslation);
                }
            }
        }
    }
}
