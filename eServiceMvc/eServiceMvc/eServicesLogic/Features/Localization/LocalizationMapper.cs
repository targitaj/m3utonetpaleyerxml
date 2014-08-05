namespace Uma.Eservices.Logic.Features.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Models.Localization;
    using dbObj = Uma.Eservices.DbObjects;
    using esrvModel = Uma.Eservices.Models.Localization;

    /// <summary>
    /// Provides methods to map between Resource (in-memory object) to Resource user which is DB object for persistence and vice-versa
    /// </summary>
    public static class LocalizationMapper
    {
        #region From eService.Model to eService.DbObjects

        /// <summary>
        /// Maps Viewmodel TranslatedTextType enum object to Db TranslatedTextType enum object.
        /// </summary>
        /// <param name="translatedTextType">ViewModel TranslatedTextType enum object</param>
        /// <returns>Db TranslatedTextType enum object</returns>
        public static dbObj.TranslatedTextType ToDbObject(this esrvModel.TranslatedTextType translatedTextType)
        {
            dbObj.TranslatedTextType returnVal;

            switch (translatedTextType)
            {
                case esrvModel.TranslatedTextType.Label:
                    returnVal = dbObj.TranslatedTextType.Label;
                    break;
                case esrvModel.TranslatedTextType.SubLabel:
                    returnVal = dbObj.TranslatedTextType.SubLabel;
                    break;
                case esrvModel.TranslatedTextType.ControlText:
                    returnVal = dbObj.TranslatedTextType.ControlText;
                    break;
                case esrvModel.TranslatedTextType.EnumText:
                    returnVal = dbObj.TranslatedTextType.EnumText;
                    break;
                case esrvModel.TranslatedTextType.HelpText:
                    returnVal = dbObj.TranslatedTextType.HelpText;
                    break;
                default:
                    throw new ArgumentException("Invalid text type");
            }

            return returnVal;
        }

        /// <summary>
        /// Maps Viewmodel SupportedLanguage enum object to Db SupportedLanguage enum object.
        /// </summary>
        /// <param name="supportedLanguage">The supported language.</param>
        /// <returns>
        /// Db SupportedLanguage enum object
        /// </returns>
        /// <exception cref="System.ArgumentException">Invalid Lang type</exception>
        public static dbObj.SupportedLanguage ToDbObject(this esrvModel.SupportedLanguage supportedLanguage)
        {
            dbObj.SupportedLanguage returnVal;

            switch (supportedLanguage)
            {
                case esrvModel.SupportedLanguage.English:
                    returnVal = dbObj.SupportedLanguage.English;
                    break;
                case esrvModel.SupportedLanguage.Finnish:
                    returnVal = dbObj.SupportedLanguage.Finnish;
                    break;
                case esrvModel.SupportedLanguage.Swedish:
                    returnVal = dbObj.SupportedLanguage.Swedish;
                    break;
                default:
                    throw new ArgumentException("Invalid Lang type");
            }

            return returnVal;
        }

        #endregion

        #region From eService.DbObjects to eService.Models

        /// <summary>
        /// Maps Db Resource object to ViewModel Resource object
        /// </summary>
        /// <param name="input">Db  Resource object</param>
        /// <param name="selectedLang">Language to load ViewModel for</param>
        /// <returns>ViewModel Resource object</returns>
        public static WebElementModel ToWebModel(this WebElement input, esrvModel.SupportedLanguage selectedLang)
        {
            if (input == null)
            {
                return null;
            }

            var model = new WebElementModel
            {
                ModelName = input.ModelName,
                PropertyName = input.PropertyName,
                WebElementId = input.WebElementId,
                SelectedLanguage = selectedLang,
                LanguageToSave = selectedLang,
                IsReturnBack = true
            };
            if (input.WebElementTranslations == null)
            {
                return model;
            }

            var translations = input.WebElementTranslations.Where(t => (byte)t.Language == (byte)selectedLang).ToList();
            if (translations.Count > 0)
            {
                foreach (WebElementTranslation webElementTranslation in translations)
                {
                    switch (webElementTranslation.TranslationType)
                    {
                        case dbObj.TranslatedTextType.Label:
                            model.PropertyLabel = webElementTranslation.TranslatedText;
                            break;
                        case dbObj.TranslatedTextType.SubLabel:
                            model.PropertySubLabel = webElementTranslation.TranslatedText;
                            break;
                        case dbObj.TranslatedTextType.ControlText:
                            model.PropertyHint = webElementTranslation.TranslatedText;
                            break;
                        case dbObj.TranslatedTextType.HelpText:
                            model.PropertyHelp = webElementTranslation.TranslatedText;
                            break;
                        case dbObj.TranslatedTextType.EnumText:
                            model.PropertyEnum = webElementTranslation.TranslatedText;
                            break;
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// Maps Db OriginalText object to ViewModel Resource object
        /// </summary>
        /// <param name="input">Db  Resource object</param>
        /// <returns>ViewModel Resource object</returns>
        public static TranslatePageModel ToWebModel(this OriginalText input)
        {
            if (input == null)
            {
                return null;
            }

            return new TranslatePageModel
                       {
                           Feature = input.Feature,
                           OriginalText = input.Original,
                       };
        }

        /// <summary>
        /// Maps Db ResourceTranslation object to ViewModel TranslatePageModeTranslation object
        /// </summary>
        /// <param name="input">Db  ResourceTranslation object</param>
        /// <returns>ViewModel ResourceTranslation object</returns>
        public static TranslatePageTranslationModel ToWebModel(this OriginalTextTranslation input)
        {
            if (input == null)
            {
                return null;
            }

            return new TranslatePageTranslationModel
            {
                Language = input.Language.ToWebModel(),
                TranslatedText = input.Translation,
            };
        }

        /// <summary>
        /// Maps Db ResourceTranslation object to ViewModel ResourceTranslation object
        /// </summary>
        /// <param name="input">Db  ResourceTranslation object</param>
        /// <returns>ViewModel ResourceTranslation object</returns>
        public static WebElementTranslationModel ToWebModel(this WebElementTranslation input)
        {
            if (input == null)
            {
                return null;
            }

            return new WebElementTranslationModel
            {
                Language = input.Language.ToWebModel(),
                WebElementId = input.WebElementId,
                TranslatedText = input.TranslatedText,
                TranslationId = input.TranslationId,
                TranslationType = input.TranslationType.ToWebModel()
            };
        }

        /// <summary>
        /// Maps Db TranslatedTextType enum object to ViewModel TranslatedTextType enum object
        /// </summary>
        /// <param name="translatedTextType">Db  TranslatedTextType enum object</param>
        /// <returns>ViewModel TranslatedTextType enum object</returns>
        public static esrvModel.TranslatedTextType ToWebModel(this dbObj.TranslatedTextType translatedTextType)
        {
            esrvModel.TranslatedTextType returnVal;

            switch (translatedTextType)
            {
                case dbObj.TranslatedTextType.Label:
                    returnVal = esrvModel.TranslatedTextType.Label;
                    break;
                case dbObj.TranslatedTextType.SubLabel:
                    returnVal = esrvModel.TranslatedTextType.SubLabel;
                    break;
                case dbObj.TranslatedTextType.ControlText:
                    returnVal = esrvModel.TranslatedTextType.ControlText;
                    break;
                case dbObj.TranslatedTextType.EnumText:
                    returnVal = esrvModel.TranslatedTextType.EnumText;
                    break;
                case dbObj.TranslatedTextType.HelpText:
                    returnVal = esrvModel.TranslatedTextType.HelpText;
                    break;
                default:
                    throw new ArgumentException("Invalid text type");
            }

            return returnVal;
        }

        /// <summary>
        /// Maps Db SupportedLanguage enum object to ViewModel SupportedLanguage enum object
        /// </summary>
        /// <param name="supportedLanguage">Db  SupportedLanguage enum object</param>
        /// <returns>ViewModel SupportedLanguage enum object</returns>
        public static esrvModel.SupportedLanguage ToWebModel(this dbObj.SupportedLanguage supportedLanguage)
        {
            esrvModel.SupportedLanguage returnVal;

            switch (supportedLanguage)
            {
                case dbObj.SupportedLanguage.English:
                    returnVal = esrvModel.SupportedLanguage.English;
                    break;
                case dbObj.SupportedLanguage.Finnish:
                    returnVal = esrvModel.SupportedLanguage.Finnish;
                    break;
                case dbObj.SupportedLanguage.Swedish:
                    returnVal = esrvModel.SupportedLanguage.Swedish;
                    break;
                default:
                    throw new ArgumentException("Invalid Lang type");
            }

            return returnVal;
        }

        /// <summary>
        /// Maps Db List of type ResourceTranslation to ViewModel List of type ResourceTranslation.
        /// </summary>
        /// <param name="input">DB List of type ResourceTranslation</param>
        /// <returns>ViewModel List of type ResourceTranslation</returns>
        public static List<WebElementTranslationModel> ToWebModel(this List<WebElementTranslation> input)
        {
            if (input == null)
            {
                return null;
            }
            var returnVal = new List<WebElementTranslationModel>();

            foreach (var item in input)
            {
                returnVal.Add(item.ToWebModel());
            }

            return returnVal;
        }

        /// <summary>
        /// Mapps List of DB- ModelTRanslation types to List of WebElementTranslationModel types
        /// </summary>
        /// <param name="input">List of ModelTranslations</param>
        /// <returns>List of WebElementranslationModel</returns>
        public static List<WebElementTranslationModel> ToWebModel(this List<ModelTranslation> input)
        {
            if (input == null)
            {
                return new List<WebElementTranslationModel>();
            }
            var returnValue = new List<WebElementTranslationModel>();

            for (int i = 0; i < input.Count; i++)
            {
                returnValue.Add(new WebElementTranslationModel
                {
                    Language = input[i].Language.ToWebModel(),
                    TranslationType = input[i].TranslationType.ToWebModel(),
                    PropertyName = input[i].PropertyName,
                    TranslatedText = input[i].TranslatedText
                });
            }
            return returnValue;
        }

        #endregion
    }
}
