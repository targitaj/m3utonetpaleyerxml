namespace Uma.Eservices.Web.Core
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using Uma.Eservices.Logic.Features.Localization;

    /// <summary>
    /// Class to use as a base class for all application Controller classes.
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Initialization of extension method executer
        /// </summary>
        private ExtensionMethodExecuter extensionMethodExecuter = new ExtensionMethodExecuter();

        /// <summary>
        /// ILocalizationManager dependency property
        /// </summary>
        [Dependency]
        public ILocalizationManager LocalizationManager { get; set; }

        /// <summary>
        /// The web messages internal variable for lazy initialization
        /// See <see cref="WebMessages"/>
        /// </summary>
        private IWebMessages webMessages;

        /// <summary>
        /// Use this property to set Web messages to show to user after page reload, like
        /// success, error or simple informative messages populated during controller code run
        /// Setter is provided so Unit tests can mock this
        /// </summary>
        public IWebMessages WebMessages
        {
            get
            {
                return this.webMessages ?? (this.webMessages = new WebMessages(this.Session));
            }

            set
            {
                this.webMessages = value;
            }
        }

        
        /// <summary>
        /// Used to execute extension methods
        /// </summary>
        public virtual ExtensionMethodExecuter ExtensionMethodExecuter
        {
            get
            {
                return extensionMethodExecuter;
            }
            set
            {
                extensionMethodExecuter = value;
            }
        }

        /// <summary>
        /// L10N = LocaliazatioN. Method used to retrieve translated string from appropriate feature resource.
        /// Supportive method to use in places where translated string is required. Use only for trusted translations.
        /// In case you need <see cref="MvcHtmlString"/> (HTML encoded string) - wrap it inside it.
        /// </summary>
        /// <param name="keyName">Name of the key to locate resource.</param>
        /// <returns>Either translated string based on CurrentUICulture or key if not found</returns>
        [ExcludeFromCodeCoverage]
        public string T(string keyName)
        {
#if PROD
            return this.LocalizationManager.GetTextTranslationPROD(keyName, this.ViewBag.FeatureName);
#else
            return this.LocalizationManager.GetTextTranslationTEST(keyName, this.ViewBag.FeatureName);
#endif
        }


        
        

        /// <summary>
        /// L10N = LocaliazatioN. Method used to retrieve translated string from appropriate feature resource.
        /// Supportive method to use in places where translated string is required. Use only for trusted translations.
        /// In case you need 
        /// <see cref="MvcHtmlString"/> (HTML encoded string) - wrap it inside it.
        /// </summary>
        /// <param name="keyName">Name of the key to locate resource.</param>
        /// <param name="args">The arguments for string Format operator.</param>
        /// <returns>Either translated string based on CurrentUICulture or key if not found</returns>
        [ExcludeFromCodeCoverage]
        public string T(string keyName, params object[] args)
        {
#if PROD
            return this.LocalizationManager.GetTextTranslationPROD(keyName, this.ViewBag.FeatureName, args);
#else
            return this.LocalizationManager.GetTextTranslationTEST(keyName, this.ViewBag.FeatureName, args);
#endif
        }

        /// <summary>
        /// Saves caller absolute url. Saves to session caller absolute url that is creating current controller
        /// </summary>
        protected void SaveCallerUrl()
        {
            this.Session["callerURL"] = Request.UrlReferrer.AbsoluteUri ?? string.Empty;
        }

        /// <summary>
        /// Used to store Controller + Action + Identifier strings where app can return in 
        /// case of "Cancel", error return and other failing circumstances.
        /// </summary>
        public ReturnControllerActionIdentifier ReturnControllerAction
        {
            get
            {
                if (this.HttpContext.Session["ReturnControllerAction"] == null)
                {
                    this.HttpContext.Session["ReturnControllerAction"] = new ReturnControllerActionIdentifier();
                }

                return (ReturnControllerActionIdentifier)this.HttpContext.Session["ReturnControllerAction"];
            }

            set
            {
                this.HttpContext.Session["ReturnControllerAction"] = value;
            }
        }

        /// <summary>
        /// Stores the return route for "Cancel" and "Back" operations.
        /// This method must be called from Controller Actions and it will automatically
        /// store route to current path.
        /// You can convert it to Filter attribute if you are certain you need storing route even if
        /// it is not beneficial due to route restrictions (security, validations)
        /// </summary>
        /// <param name="bookmarkTag">Optional: The bookmark tag if you need additional page place handling (like preselecting/opening panel, selecting tab, scrolling to bookmark etc.).</param>
        protected void StoreReturnRoute(string bookmarkTag = null)
        {
            string controllerName = null;
            string actionName = null;
            string idValue = null;

            if (this.ControllerContext.RouteData.Values.ContainsKey("controller"))
            {
                controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            }

            if (this.ControllerContext.RouteData.Values.ContainsKey("action"))
            {
                actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            }

            if (this.ControllerContext.RouteData.Values.ContainsKey("id"))
            {
                idValue = this.ControllerContext.RouteData.Values["id"].ToString();
            }

            var returnRoute = new ReturnControllerActionIdentifier { ControllerName = controllerName, ActionName = actionName, EntityId = idValue };
            if (!string.IsNullOrEmpty(bookmarkTag))
            {
                returnRoute.BookmarkTag = bookmarkTag;
            }

            this.ReturnControllerAction = returnRoute;
        }

        /// <summary>
        /// Redirects the back to specified Controller/Action/ID/Bookmark
        /// If values are not supplied, redirects to path, saved in session via <seealso cref="ReturnControllerAction"/>
        /// </summary>
        /// <param name="controllerName">The controller name.</param>
        /// <param name="actionName">The action name (for Controller).</param>
        /// <param name="returnIdValue">The return id.</param>
        /// <param name="bookmarkTag">Bookmarks of place in page where it should scroll to. It must be handled separately from Route parameters in actions/views</param>
        /// <param name="absoluteReturnLink">Absolute Url that is used to redirect back to page.</param>
        public virtual ActionResult RedirectBack(string controllerName = null, string actionName = null, string returnIdValue = null, string bookmarkTag = null, string absoluteReturnLink = null)
        {
            if (!string.IsNullOrEmpty(bookmarkTag))
            {
                this.TempData["bookmarkTag"] = bookmarkTag;
            }

            if (!string.IsNullOrEmpty(absoluteReturnLink))
            {
                return this.Redirect(absoluteReturnLink);
            }

            if (string.IsNullOrEmpty(controllerName))
            {
                controllerName = this.ReturnControllerAction.ControllerName;
                actionName = this.ReturnControllerAction.ActionName;
                returnIdValue = this.ReturnControllerAction.EntityId;
            }

            if (string.IsNullOrEmpty(controllerName))
            {
                return this.RedirectToRoute("Default", new { controller = MVC.Home.Name, action = MVC.Home.ActionNames.Index });
            }

            if (string.IsNullOrEmpty(actionName))
            {
                return this.RedirectToRoute("Default", new { controller = controllerName, action = "Index" });
            }

            if (string.IsNullOrEmpty(returnIdValue))
            {
                return this.RedirectToRoute("Default", new { controller = controllerName, action = actionName });
            }

            return this.RedirectToRoute("Default", new { controller = controllerName, action = actionName, id = returnIdValue });
        }

        /// <summary>
        /// Method to have access to protected Json method
        /// </summary>
        /// <param name="data">Object for serialization</param>
        public virtual JsonResult JsonPublic(object data)
        {
            return Json(data);
        }

        /// <summary>
        /// Method to have access to protected File method
        /// </summary>
        /// <param name="fileName">File name for downloading</param>
        /// <param name="contentType">File content type</param>
        public virtual FilePathResult FilePublic(string fileName, string contentType)
        {
            return File(fileName, contentType);
        }
    }
}