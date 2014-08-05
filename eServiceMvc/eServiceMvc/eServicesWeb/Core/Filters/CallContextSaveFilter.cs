namespace Uma.Eservices.Web.Core.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using Newtonsoft.Json;
    using Uma.Eservices.Common;

    /// <summary>
    /// Filter to save action calling context to add to exception if thrown during execution
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CallContextSaveFilterAttribute : FilterAttribute, IActionFilter
    {
        /// <summary>
        /// Holds the logger component.
        /// </summary>
        [Dependency]
        public ILog Logger { get; set; }

        /// <summary>
        /// Called before an action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                return;
            }

            string controllerName = filterContext.RouteData.Values["controller"].ToString();
            string actionName = filterContext.RouteData.Values["action"].ToString();
#if DEBUG
            this.Logger.Trace(string.Concat("Calling ", controllerName, "/", actionName));
#endif
            if (filterContext.ActionParameters.Count <= 0)
            {
                return;
            }

            StringBuilder contextValues = new StringBuilder();
            foreach (KeyValuePair<string, object> parameter in filterContext.ActionParameters)
            {
                string json = JsonConvert.SerializeObject(parameter.Value);
                if (parameter.Value == null)
                {
                    contextValues.AppendLine(string.Concat("object ", parameter.Key, ": NULL"));
                }
                else
                {
                    string type = parameter.Value.GetType().Name;
                    contextValues.AppendLine(string.Concat(type, " ", parameter.Key, ": ", json.Length < 3073 ? json : json.Substring(0, 1024)));
                }
            }

            // Store it in ViewBag, so if exception occurs in subsequent controller action, exception handler can use it to store info on calling parameters
            filterContext.Controller.ViewBag.CallingContext = contextValues.ToString();
#if DEBUG
            this.Logger.Trace(string.Concat("Calling ", controllerName, "/", actionName), contextValues.ToString());
#endif
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