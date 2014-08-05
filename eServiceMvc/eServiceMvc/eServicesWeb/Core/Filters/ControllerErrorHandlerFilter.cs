namespace Uma.Eservices.Web.Core.Filters
{
    using System;
    using System.Data.Entity.Validation;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.Practices.Unity;
    using Uma.Eservices.Common;

    /// <summary>
    /// Handles all exceptions occuring in Controllers and their instantiated objects
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ControllerErrorHandlerFilterAttribute : FilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// Holds the logger component.
        /// </summary>
        [Dependency]
        public ILog Logger { get; set; }

        /// <summary>
        /// Called when an exception occurs.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnException(ExceptionContext filterContext)
        {
            // Don't bother if custom errors are turned off or if the exception is already handled
            if (filterContext == null || filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            // TODO: Add context variables and User data to exception
            string errorMsg = string.Concat(
                "Error occured in ", filterContext.RouteData.Values["controller"].ToString(), "/", filterContext.RouteData.Values["action"].ToString());

            if (filterContext.Controller.ViewBag.CallingContext != null)
            {
                this.Logger.Error(errorMsg, filterContext.Exception, filterContext.Controller.ViewBag.CallingContext);
            }
            else
            {
                this.Logger.Error(errorMsg, filterContext.Exception);
            }

            if (filterContext.Exception is DbEntityValidationException)
            {
                this.CheckForEntityError(filterContext);
            }

            var httpException = filterContext.Exception as HttpException;

            // Set the view correctly depending if it's an AJAX request or not
            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        error = true,
                        message = filterContext.Exception.Message
                    }
                };
            }
            else
            {
                if (httpException != null && httpException.GetHttpCode() == 404)
                {
                    filterContext.HttpContext.Response.StatusCode = 404;
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "NotFound" } });
                }
                else
                {
                    var controller = (BaseController)filterContext.Controller;
                    // TODO: refine this message, add button with "report" link to send cry for help?
                    controller.WebMessages.AddErrorMessage(
                        "Operation ended in error.",
                        "You can try to do operation once more. If problem persists, please report it to Web master by pressing a button here",
                        "~/home/index",
                        "Report");
                    filterContext.Result = controller.RedirectBack();
                    filterContext.HttpContext.Response.StatusCode = 500;
                }
            }

            filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

        /// <summary>
        /// Tests entity whether it contains any errors
        /// </summary>
        /// <param name="filterContext">MVC Filter context object</param>
        private void CheckForEntityError(ExceptionContext filterContext)
        {
            var entErrorMsg = string.Empty;

            foreach (var eve in (filterContext.Exception as DbEntityValidationException).EntityValidationErrors)
            {
                entErrorMsg +=
                    string.Format(CultureInfo.InvariantCulture,
                        "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name,
                        eve.Entry.State);

                entErrorMsg = eve.ValidationErrors.Aggregate(
                    entErrorMsg,
                    (current, ve) =>
                    current + string.Format(CultureInfo.InvariantCulture, "- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
            }

            this.Logger.Error(entErrorMsg);
        }
    }
}