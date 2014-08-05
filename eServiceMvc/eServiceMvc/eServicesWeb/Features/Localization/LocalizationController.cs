namespace Uma.Eservices.Web.Features.Localization
{
    using System;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using Uma.Eservices.Common;
    using Uma.Eservices.Logic.Features;
    using Uma.Eservices.Logic.Features.HelpSupport;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Account;
    using Uma.Eservices.Models.Localization;
    using Uma.Eservices.Web.Core;
    using Uma.Eservices.Web.Core.Filters;

    /// <summary>
    /// Localization controller class
    /// used to manage (CRUD) translate Poperties in WebModels
    /// </summary>
    public partial class LocalizationController : BaseController
    {
        /// <summary>
        /// Gets or sets the ILocalizationEditor component of Controller(s).
        /// </summary>
        [Dependency]
        public ILocalizationEditor LocalEditor { get; set; }

        /// <summary>
        /// Gets or sets the logger component of Controller(s).
        /// </summary>
        [Dependency]
        public ILog Logger { get; set; }

        /// <summary>
        /// Gets or sets the IHelpSupportLogic component of Controller(s).
        /// </summary>
        [Dependency]
        public IHelpSupportLogic HelpLogic { get; set; }

        /// <summary>
        /// Returns LocalizationManager with OriginalText model
        /// </summary>
        /// <param name="text">Text to be translated</param>
        /// <param name="feature">Feature for translation</param>
        /// <param name="selectedLang">Selected language value</param>
        /// <param name="saveCallerUrl">Used for redirecting to correct previous page after saving</param>
        [Authorize(Roles = ApplicationRoles.Translator)]
        [HttpGet]
        [ValidateInput(false)]
        public virtual ActionResult TextTranslationByLanguage(string text, string feature, SupportedLanguage selectedLang, bool saveCallerUrl = false)
        {
            var model = this.LocalEditor.GetTranslatePageModel(text, feature, selectedLang);
            if (saveCallerUrl)
            {
                base.SaveCallerUrl();
            }

            return this.View(MVC.Localization.Views.OriginalTextTranslation, model);
        }

        /// <summary>
        /// Save TranslatePageModel model
        /// </summary>
        /// <param name="model">TranslatePageModel model instance</param>
        [Authorize(Roles = ApplicationRoles.Translator)]
        [DatabaseTransaction]
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult TextTranslation(TranslatePageModel model)
        {
            if (model == null)
            {
                throw new ArgumentException("Passed model (TranslatePageModel) is null");
            }

            this.LocalEditor.AddUpdateOriginalText(model);
            if (model.IsReturnBack)
            {
                string absoluteReturnUrl = this.Session["callerURL"].ToString();
                this.Session.Remove("callerURL");
                return base.RedirectBack(absoluteReturnLink: absoluteReturnUrl);
            }

            return this.RedirectToAction(MVC.Localization.TextTranslationByLanguage(model.OriginalText, model.Feature, model.SelectedLanguage));
        }

        /// <summary>
        /// Returns LocalizationManager with WebElement model
        /// </summary>
        /// <param name="modelName">Model name</param>
        /// <param name="propertyName">Model property name</param>
        /// <param name="selectedLang">The selected language for translations.</param>
        /// <param name="saveCallerUrl">Used for redirecting to correct previous page after saving</param>
        [Authorize(Roles = ApplicationRoles.Translator)]
        [HttpGet]
        public virtual ActionResult Resources(string modelName, string propertyName, SupportedLanguage selectedLang, bool saveCallerUrl = false)
        {
            var model = this.LocalEditor.GetResource(propertyName, modelName, selectedLang);
            if (saveCallerUrl)
            {
                base.SaveCallerUrl();
            }

            return this.View(MVC.Localization.Views.FieldLabelTranslation, model);
        }

        /// <summary>
        /// Save WebElement model
        /// </summary>
        /// <param name="model">WebElemnt model instance</param>
        [Authorize(Roles = ApplicationRoles.Translator)]
        [HttpPost]
        [DatabaseTransaction]
        public virtual ActionResult Resources(WebElementModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model", "Passed model (WebElementModel) is null");
            }

            this.LocalEditor.SaveResources(model);

            if (model.IsReturnBack)
            {
                string absoluteReturnUrl = this.Session["callerURL"].ToString();
                this.Session.Remove("callerURL");
                return base.RedirectBack(absoluteReturnLink: absoluteReturnUrl);
            }

            return this.RedirectToAction(MVC.Localization.Resources(model.ModelName, model.PropertyName, model.SelectedLanguage));
        }

        /// <summary>
        /// FAQs the translations by language.
        /// </summary>
        /// <param name="faqId">The identifier.</param>
        /// <param name="selectedLang">The selected language.</param>
        [Authorize(Roles = ApplicationRoles.Translator)]
        [HttpGet]
        public virtual ActionResult FAQTranslationsByLanguage(int faqId, SupportedLanguage selectedLang)
        {
            return this.View(MVC.Localization.Views.FAQTranslation, this.HelpLogic.GetFAQResources(faqId, selectedLang));
        }

        /// <summary>
        /// Returns translation for FAQ items
        /// </summary>
        /// <param name="faqId">The identifier of FAQ item.</param>
        /// <param name="saveCallerUrl">Save caller return url</param>
        [Authorize(Roles = ApplicationRoles.Translator)]
        [HttpGet]
        public virtual ActionResult FAQTranslations(int faqId, bool saveCallerUrl = true)
        {
            if (saveCallerUrl)
            {
                base.SaveCallerUrl();
            }
            return this.RedirectToAction(MVC.Localization.FAQTranslationsByLanguage(faqId, Globalizer.CurrentUICultureLanguage.Value));
        }

        /// <summary>
        /// Creates new Faq redirects to translation page
        /// </summary>
        /// <param name="saveCellerUrl">Save caller return url</param>
        [Authorize(Roles = ApplicationRoles.Translator)]
        [DatabaseTransaction]
        [HttpGet]
        public virtual ActionResult CreateNewFaq(bool saveCellerUrl = true)
        {
            if (saveCellerUrl)
            {
                base.SaveCallerUrl();
            }

            var res = this.HelpLogic.CreateNewfaq();
            return this.RedirectToAction(MVC.Localization.FAQTranslations(res));
        }

        /// <summary>
        /// Updates or creates new TranslateFAQPageModel in db
        /// </summary>
        /// <param name="model">TranslateFAQPageModel model</param>
        [Authorize(Roles = ApplicationRoles.Translator)]
        [HttpPost]
        [DatabaseTransaction]
        public virtual ActionResult UpdateFAQ(TranslateFAQPageModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Passed model (WebElementModel) is null");
            }

            this.HelpLogic.CreateUpdateFAQ(model);

            if (model.IsReturnBack)
            {
                string absoluteReturnUrl = this.Session["callerURL"].ToString();
                this.Session.Remove("callerURL");
                return base.RedirectBack(absoluteReturnLink: absoluteReturnUrl);
            }

            return this.RedirectToAction(MVC.Localization.FAQTranslationsByLanguage(model.FaqId, model.SelectedLanguage));
        }

        /// <summary>
        /// Creates new Faq redirects to translation page
        /// </summary>
        /// <param name="questionId">Unique FaqTranslation id</param>
        [Authorize(Roles = ApplicationRoles.Translator)]
        [DatabaseTransaction]
        [HttpGet]
        public virtual ActionResult DeleteFaqTranslation(int questionId)
        {
            this.HelpLogic.DeleteQuestion(questionId);
            return this.RedirectToAction(MVC.Info.Help());
        }
    }
}