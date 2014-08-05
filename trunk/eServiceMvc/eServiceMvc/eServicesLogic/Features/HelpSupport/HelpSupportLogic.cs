namespace Uma.Eservices.Logic.Features.HelpSupport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Localization;
    using SupportedLanguage = Uma.Eservices.Models.Localization.SupportedLanguage;

    /// <summary>
    /// IHelpSupportLogic interface implementation
    /// </summary>
    public class HelpSupportLogic : IHelpSupportLogic
    {
        /// <summary>
        /// Gets basic Db operation querys.
        /// </summary>
        private IGeneralDataHelper DbContext { get; set; }

        /// <summary>
        /// Class contructor set DbContext property
        /// </summary>
        /// <param name="dbContext">IGeneralDataHelper instance</param>
        public HelpSupportLogic(IGeneralDataHelper dbContext)
        {
            this.DbContext = dbContext;
        }

        /// <summary>
        /// Returns all questions answers from db, by specified language
        /// </summary>
        /// <param name="lang">Supported language for questions and answers</param>>
        /// <returns>HelpPageModel object</returns>
        public List<TranslateFAQPageModel> GetAllQuestionsAllAnswers(SupportedLanguage lang)
        {
            var langParam = LocalizationMapper.ToDbObject(lang);
            var res = this.DbContext.GetMany<FaqTranslation>(o => o.Language == langParam).ToList<FaqTranslation>();
            return HelpSupportMapper.ToWeb(res);
        }

        /// <summary>
        /// Returns all questions answers from db, by default language
        /// </summary>
        /// <returns>HelpPageModel object</returns>
        public List<TranslateFAQPageModel> GetAllQuestionsAllAnswers()
        {
            return this.GetAllQuestionsAllAnswers(Globalizer.CurrentUICultureLanguage.Value);
        }

        /// <summary>
        /// Creates new FAq model in DB, returns faq ID
        /// </summary>
        /// <returns>Faq model id</returns>
        public int CreateNewfaq()
        {
            Faq tempF = new Faq(){Description = ""};
            this.DbContext.Create<Faq>(tempF);
            this.DbContext.FlushChanges();
            return tempF.Id;
        }

        /// <summary>
        /// Method creates new or updates existing FAQ model and it childrens in db
        /// </summary>
        /// <param name="model">TranslateFAQPageModel object</param>
        public void CreateUpdateFAQ(TranslateFAQPageModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("FaqModel model is null");
            }
            var faqDb = this.DbContext.Get<Faq>(o => o.Id == model.FaqId);

            if (model.Id == 0 && !string.IsNullOrEmpty(model.Question) && !string.IsNullOrEmpty(model.Answer))
            {
                faqDb.FaqTranslations.Add(HelpSupportMapper.ToFaqTransDbObject(model));
            }

            if (model.Id > 0)
            {
                var tempModel = faqDb.FaqTranslations.Where(o => o.Id == model.Id).FirstOrDefault();
                tempModel.Question = model.Question;
                tempModel.Answer = model.Answer;
            }

            this.DbContext.Update<Faq>(faqDb);
        }

        /// <summary>
        /// Returns specific
        /// </summary>
        /// <param name="faqId">DB Id for FAQ record</param>
        /// <param name="supportedLanguage">FAQ language</param>
        public TranslateFAQPageModel GetFAQResources(int faqId, SupportedLanguage supportedLanguage)
        {
            if (faqId == 0)
            {
                return null;
            }

            var tempLang = supportedLanguage.ToDbObject();
            // Get all by id
            var faqTransModel = this.DbContext.Get<Faq>(o => o.Id == faqId).FaqTranslations.Where(o => o.Language == tempLang).FirstOrDefault();

            if (faqTransModel == null)
            {
                faqTransModel = new FaqTranslation
                {
                    Language = tempLang,
                    FaqId = faqId
                };
            }

            TranslateFAQPageModel returnVal = HelpSupportMapper.ToWeb(faqTransModel);

            return returnVal;
        }

        /// <summary>
        /// Method deletes question by given id
        /// </summary>
        /// <param name="questionId">Question unique id</param>
        public void DeleteQuestion(int questionId)
        {
            var faqDbTranslation = this.DbContext.Get<FaqTranslation>(o => o.Id == questionId);

            this.DbContext.Delete<FaqTranslation>(faqDbTranslation);
        }
    }
}
