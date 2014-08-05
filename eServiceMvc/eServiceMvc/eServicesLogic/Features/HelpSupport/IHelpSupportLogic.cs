namespace Uma.Eservices.Logic.Features.HelpSupport
{
    using System.Collections.Generic;
    using Uma.Eservices.Models.Localization;
    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// IHelpSupportLogic contains logic for managing questions / answers
    /// </summary>
    public interface IHelpSupportLogic
    {
        /// <summary>
        /// Returns all questions answers from db, by specified language
        /// </summary>
        /// <param name="lang">Supported language for questions and answers</param>>
        /// <returns>HelpPageModel object</returns>
        List<TranslateFAQPageModel> GetAllQuestionsAllAnswers(SupportedLanguage lang);

        /// <summary>
        /// Returns all questions answers from db, by default language
        /// </summary>
        /// <returns>HelpPageModel object</returns>
        List<TranslateFAQPageModel> GetAllQuestionsAllAnswers();

        /// <summary>
        /// Creates new FAq model in DB, returns faq ID
        /// </summary>
        /// <returns>Faq model id</returns>
        int CreateNewfaq();

        /// <summary>
        /// Method creates new or updates existing FAQ model in db
        /// </summary>
        /// <param name="model">TranslateFAQPageModel object</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "FAQ")]
        void CreateUpdateFAQ(TranslateFAQPageModel model);

        /// <summary>
        /// Returns  FAQ records from db by specified id and laguage
        /// </summary>
        /// <param name="faqId">Faq id stored in DB</param>
        /// <param name="supportedLanguage">Filter record vie language</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "FAQ")]
        TranslateFAQPageModel GetFAQResources(int faqId, SupportedLanguage supportedLanguage);

        /// <summary>
        /// Method deletes question by given id
        /// </summary>
        /// <param name="questionId">Question unique id</param>
        void DeleteQuestion(int questionId);
    }
}
