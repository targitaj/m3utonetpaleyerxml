namespace Uma.Eservices.Web.Core
{
    using System.Web;
    using System.Web.Mvc;
    using Uma.Eservices.Common;

    /// <summary>
    /// Interface to abstract an Base Controller and force derived classes to implement certain functionalities
    /// </summary>
    public interface IBaseController
    {
        /// <summary>
        /// Gets or sets the logger component of Controller(s).
        /// </summary>
        ILog Logger { get; set; }

        /// <summary>
        /// L10N = LocaliazatioN. Method used to retrieve translated string from appropriate feature resource.
        /// Supportive method to use in places where translated string is required. Use only for trusted translations.
        /// </summary>
        /// <param name="keyName">Name of the key to locate resource.</param>
        /// <returns>Either translated string based on CurrentUICulture or key if not found</returns>
        string T(string keyName);

        /// <summary>
        /// Gets or sets the object which holds return patch for "Cancel" and "Back" operations
        /// </summary>
        ReturnControllerActionIdentifier ReturnControllerAction { get; set; }

        /// <summary>
        /// Operation which is capable of reloading operation specified either in parameter
        /// or - if called without parameters - use internal <see cref="ReturnControllerAction"/> variable data to 
        /// return to previously saved location.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="returnIdValue">The return identifier value.</param>
        /// <param name="bookmarkTag">The bookmark tag - place in target screen or operable value to pre-select something in screen (like Tab).</param>
        /// <param name="absoluteReturnLink">Absolute Url that is used to redirect back to page.</param>
        ActionResult RedirectBack(string controllerName = null, string actionName = null, string returnIdValue = null, string bookmarkTag = null, string absoluteReturnLink = null);
    }
}