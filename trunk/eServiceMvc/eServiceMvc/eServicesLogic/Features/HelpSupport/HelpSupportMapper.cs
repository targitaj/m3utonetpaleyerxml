namespace Uma.Eservices.Logic.Features.HelpSupport
{
    using System;
    using System.Collections.Generic;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Localization;

    /// <summary>
    /// Static Help support object mapper
    /// </summary>
    public static class HelpSupportMapper
    {
        #region From WEB to DB

        /// <summary>
        /// Maps TranslateFAQPageModel web model to Faq db Model
        /// Creates new List of FaqTrasnlation children for Faq model
        /// </summary>
        /// <param name="model">TranslateFAQPageModel Web Model</param>
        /// <returns>Faq Db Model</returns>
        public static Faq ToFaqDbObject(this TranslateFAQPageModel model)
        {
            List<FaqTranslation> tempList = new List<FaqTranslation>();
            tempList.Add(new FaqTranslation
            {
                Language = model.LanguageToSave.ToDbObject(),
                Question = model.Question,
                Answer = model.Answer
            });

            return new Faq
            {
                FaqTranslations = tempList
            };
        }

        /// <summary>
        /// Maps TranslateFAQPageModel web model to FaqTranslation db Model
        /// </summary>
        /// <param name="model">TranslateFAQPageModel Web Model</param>
        /// <returns>FaqTranslation Db Model</returns>
        public static FaqTranslation ToFaqTransDbObject(this TranslateFAQPageModel model)
        {
            return new FaqTranslation
            {
                Answer = model.Answer,
                Question = model.Question,
                Language = model.LanguageToSave.ToDbObject()
            };
        }
        #endregion

        #region From DB to WEB

        /// <summary>
        /// Maps  FaqTranslation -> db TO TranslateFAQPageModel -> Web model
        /// </summary>
        /// <param name="input">FaqTranslation object instance</param>
        /// <returns>TranslateFAQPageModel object instance</returns>
        public static TranslateFAQPageModel ToWeb(this FaqTranslation input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("FaqTranslation model is null");
            }

            return new TranslateFAQPageModel
            {
                Id = input.Id,
                FaqId = input.FaqId,
                Answer = input.Answer,
                Question = input.Question,
                IsReturnBack = true,
                LanguageToSave = LocalizationMapper.ToWebModel(input.Language),
                SelectedLanguage = LocalizationMapper.ToWebModel(input.Language)
            };
        }

        /// <summary>
        /// Maps  List FaqTranslation -> db TO List FaqTranslationModel -> Web model
        /// </summary>
        /// <param name="input">List FaqTranslation object instance</param>
        /// <returns>List TranslateFAQPageModel object instance</returns>
        public static List<TranslateFAQPageModel> ToWeb(this List<FaqTranslation> input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("List<FaqTranslation> model is null");
            }

            var returnVal = new List<TranslateFAQPageModel>();

            for (int i = 0; i < input.Count; i++)
            {
                returnVal.Add(ToWeb(input[i]));
            }

            return returnVal;
        }
        #endregion
    }
}
