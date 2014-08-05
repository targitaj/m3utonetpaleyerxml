namespace Uma.Eservices.Web.Core
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc;

    using Glimpse.Mvc.AlternateType;
    using Microsoft.Practices.Unity;

    using Uma.Eservices.Logic.Features;
    using Uma.Eservices.Logic.Features.Common;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Account;
    using Uma.Eservices.Models.Localization;
    using Uma.Eservices.Web.Components;

    /// <summary>
    /// A base view providing an alias for localizable resources
    /// </summary>
    /// <typeparam name="TModel">Strongly typed view Model type</typeparam>
    public abstract class BaseView<TModel> : WebViewPage<TModel>
    {
        /// <summary>
        /// Private WebElementLocalizer property, contains list of available property translations in model
        /// </summary>
        private WebElementLocalizer elementLocalizationHelper;

        /// <summary>
        /// ILocalizationManager dependency property
        /// </summary>
        [Dependency]
        public ILocalizationManager LocalizationManager { get; set; }

        /// <summary>
        /// Contains overal UMA collections
        /// </summary>
        [Dependency]
        public IGetUmaCollections Collections { get; set; }

        /// <summary>
        /// WebElementLocalizer property returns list of available property translations in model
        /// </summary>
        public virtual WebElementLocalizer WebElementTranslations
        {
            get
            {
                if (this.elementLocalizationHelper == null)
                {
                    string modelName = UmaHtmlHelpers.ExtractModelName(typeof(TModel).FullName);
                    this.elementLocalizationHelper = this.LocalizationManager.GetWebElementTranslations(modelName);
                }

                return this.elementLocalizationHelper;
            }
        }

        /// <summary>
        /// L10N = LocaliazatioN. Method used to retrieve translated string from appropriate feature resource.
        /// Helper method for getting translations to UI. Returns HTML encoded string (prevents XSS attacks)
        /// </summary>
        /// <param name="keyText">The original text (as a key).</param>
        /// <param name="args">The arguments of a text.</param>
        /// <returns>
        /// Localized/resourced string or key if not found
        /// </returns>
        public IHtmlString T(string keyText, params object[] args)
        {
            return BaseView.GetTextTranslation(keyText, this.LocalizationManager, this.Url, this.ViewBag, args);
        }

        /// <summary>
        /// Inject View Data Dictionary to view
        /// </summary>
        /// <typeparam name="T">Type of Dictionary</typeparam>
        /// <param name="viewDataDictionary">View data dictionary object</param>
        public void SetViewData<T>(ViewDataDictionary<T> viewDataDictionary)
        {
            base.SetViewData(viewDataDictionary);
        }
    }

    /// <summary>
    /// A base view providing an alias for localizable resources
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Too stupid to have two files for same purpose.")]
    public abstract class BaseView : WebViewPage
    {
        /// <summary>
        /// ILocalizationManager dependency property
        /// </summary>
        [Dependency]
        public ILocalizationManager LocalizationManager { get; set; }

        /// <summary>
        /// L10N = LocaliazatioN. Method used to retrieve translated string from appropriate feature resource.
        /// Helper method for getting translations to UI. Returns HTML encoded string (prevents XSS attacks)
        /// </summary>
        /// <param name="keyText">The original text (as a key).</param>
        /// <param name="args">The arguments of a text.</param>
        /// <returns>Localized string or key if not found</returns>
        public IHtmlString T(string keyText, params object[] args)
        {
            return BaseView.GetTextTranslation(keyText, this.LocalizationManager, this.Url, this.ViewBag, args);
        }

        /// <summary>
        /// Used for text translation and link creation for translation editing purpose
        /// </summary>
        /// <param name="keyText">The original text (as a key).</param>
        /// <param name="localizationManager">ILocalizationManager dependency property</param>
        /// <param name="urlHelper">Helper for url generation</param>
        /// <param name="viewBag">View bag for Feature receiving</param>
        /// <param name="args">The arguments of a text.</param>
        internal static IHtmlString GetTextTranslation(string keyText, ILocalizationManager localizationManager, UrlHelper urlHelper, dynamic viewBag, params object[] args)
        {
#if PROD
            return new MvcHtmlString(localizationManager.GetTextTranslationPROD(keyText, viewBag.FeatureName, args: args));
#else
            var translation = localizationManager.GetTextTranslationTEST(keyText, viewBag.FeatureName, args: args);

            if (HttpContext.Current.User.IsInRole(ApplicationRoles.Translator))
            {
                var url = urlHelper.Action(
                MVC.Localization.ActionNames.TextTranslationByLanguage,
                MVC.Localization.Name,
                new
                {
                    feature = viewBag.FeatureName,
                    text = keyText,
                    selectedLang = Globalizer.CurrentUICultureLanguage.Value,
                    saveCallerUrl = true
                });

                translation = localizationManager.FormatTranslationForTranslator(translation, url);
            }

            return new MvcHtmlString(translation);
#endif
        }
    }
}