namespace Uma.Eservices.Web.Core.Filters
{
    using System;
    using System.Web.Mvc;

    using Uma.Eservices.Logic.Features.Localization;

    /// <summary>
    /// Creates FeatureName property for ViewBag dynamic object
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class FeatureNameFilterAttribute : FilterAttribute, IActionFilter
    {
        /// <summary>
        /// Called before an action method executes.
        /// Creates FeatureName property for ViewBag dynamic object
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                return;
            }

            string contextNamespace = filterContext.Controller.GetType().Namespace;

            // Reusing Feature name extraction to align ways of finding translation both in create and extract times
            string featureName = LocalizationManager.GetFeatureNameFromNamespace(contextNamespace);
            filterContext.Controller.ViewBag.FeatureName = featureName;
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