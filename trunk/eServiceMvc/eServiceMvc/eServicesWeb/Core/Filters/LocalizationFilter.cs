namespace Uma.Eservices.Web.Core.Filters
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using Uma.Eservices.Common;

    /// <summary>
    /// Filter is setting up: 
    /// 1) Thread.CurrentThread.CurrentUICulture to one, which is specified in GLOBALIZER class and has translations (locatable in List of implemented cultures)
    ///    This culture is responsible for UI language - translations
    /// 2) Thread.CurrentThread.CurrentCulture to one, which is most appropriate for requested/retrievable from context.
    ///    This is responsible for formatting dates, numbers etc in interface - globalization thingies.
    ///
    /// Filter looks for Culture in these places (in specified order):
    /// 1) In Request Route (specified, like "EN" here http://www.site.com/EN/controller/action/id) - language changing action
    /// 2) User preferences (if user has logged in and has such preferences - most probably in some Identity, easily available in context)
    /// 3) From request Cookie (saved in previus requests)
    /// 4) From Browser settings - language prioritization in request from used browser.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    [ExcludeFromCodeCoverage]
    public class LocalizationFilter : FilterAttribute, IActionFilter
    {
        /// <summary>
        /// Holds the logger component.
        /// </summary>
        [Dependency]
        public ILog Logger { get; set; }

        /// <summary>
        /// Called before an action method executes.
        /// Uses internal methods to set both cultures for subsequent code executions (in controller)
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Thread.CurrentThread.CurrentUICulture = CultureHelper.ResolveUICulture(filterContext);
            Thread.CurrentThread.CurrentCulture = CultureHelper.ResolveCulture(filterContext);
            this.Logger.Debug("Culture: {0}; UI: {1}", Thread.CurrentThread.CurrentCulture.Name, Thread.CurrentThread.CurrentUICulture.Name);
        }

        /// <summary>
        /// Called after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}