namespace Uma.Eservices.Web.Features.Home
{
    using System.Globalization;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using Uma.Eservices.Common;
    using Uma.Eservices.Logic.Features;
    using Uma.Eservices.Models.Home;
    using Uma.Eservices.Models.Shared;
    using Uma.Eservices.Web.Core;

    /// <summary>
    /// Home - Controller responsible for common functionalities and main page
    /// </summary>
    public partial class HomeController : BaseController, IBaseController
    {
        /// <summary>
        /// Gets or sets the logger component of Controller(s).
        /// </summary>
        [Dependency]
        public ILog Logger { get; set; }

        /// <summary>
        /// Main entry point for whole application
        /// </summary>
        [AllowAnonymous]
        public virtual ViewResult Index()
        {
            return this.View(MVC.Home.Views.Index);
        }

        /// <summary>
        /// Renders partial view of Top level navigation (main menu)
        /// </summary>
        [AllowAnonymous]
        public virtual PartialViewResult TopNavigation()
        {
            var model = new TopNavigationModel { CurrentLanguage = Globalizer.GetNeutralCulture(Thread.CurrentThread.CurrentUICulture.Name).ToUpperInvariant() };

            // Getting parent controller action to act if it is specific one (TODO: If it gets more complicated - move to logic!)
            var rd = ControllerContext.ParentActionViewContext.RouteData;
            var currentAction = rd.GetRequiredString("action");
            var currentController = rd.GetRequiredString("controller");
            if (!string.IsNullOrEmpty(currentAction) && !string.IsNullOrEmpty(currentController))
            {
                // For Login/Register screens disable Login link
                if (currentController.ToUpperInvariant().Equals("ACCOUNT")
                    && (currentAction.ToUpperInvariant().Equals("LOGIN") || currentAction.ToUpperInvariant().Equals("REGISTER")))
                {
                    model.AuthenticationDisabled = true;
                }

                // for Localization screens disable language selector
                if (currentController.ToUpperInvariant().Equals("LOCALIZATION"))
                {
                    model.LanguagesDisabled = true;
                }
            }

            return this.PartialView(MVC.Shared.Views._TopNavigation, model);
        }

        /// <summary>
        /// Sets the english language for session in site.
        /// </summary>
        [AllowAnonymous]
        public virtual ActionResult SetEnglish()
        {
            return this.SetLanguage("en-US");
        }

        /// <summary>
        /// Sets the finnish language for session in site.
        /// </summary>
        [AllowAnonymous]
        public virtual ActionResult SetFinnish()
        {
            return this.SetLanguage("fi-FI");
        }

        /// <summary>
        /// Sets the swedish language for session in site.
        /// </summary>
        [AllowAnonymous]
        public virtual ActionResult SetSwedish()
        {
            return this.SetLanguage("sv-SE");
        }

        /// <summary>
        /// Method to actually set specific language and reroute back to previosuly selected page
        /// </summary>
        /// <param name="language">Language to select</param>
        /// <returns>If Http context has referrer - return to it, otherwise go to Home/Index</returns>
        private ActionResult SetLanguage(string language)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(language);
            this.Response.Cookies.Add(new HttpCookie(CultureHelper.UiCookieName) { Value = language });
            var urlReferrer = this.HttpContext.Request.UrlReferrer;
            if (urlReferrer != null)
            {
                return this.Redirect(urlReferrer.AbsoluteUri);
            }

            return this.View(MVC.Home.Views.Index);
        }

        /// <summary>
        /// Renders partial view of footer for pages
        /// </summary>
        [AllowAnonymous]
        public virtual PartialViewResult Footer()
        {
            return this.PartialView(MVC.Shared.Views._footer);
        }

        /// <summary>
        /// Action to call in case of unhandled exception
        /// </summary>
        /// <returns>Page Not Found page, but better than ASP.NET default one :-)</returns>
        [AllowAnonymous]
        public virtual ViewResult NotFound()
        {
            return this.View(MVC.Home.Views.ViewNames.PageNotFound);
        }

        /// <summary>
        /// Actio to prepare and render partial view showing all kinds of mesages to user in page.
        /// Must go to Master page, as this is global partial view.
        /// </summary>
        /// <returns>Empty string (if no messages) or partial view</returns>
        [AllowAnonymous]
        public virtual PartialViewResult ShowWebMessages()
        {
            WebMessagesModel model = new WebMessagesModel(this.WebMessages.Messages);
            return this.PartialView(MVC.Home.Views._WebMessages, model);
        }
    }
}