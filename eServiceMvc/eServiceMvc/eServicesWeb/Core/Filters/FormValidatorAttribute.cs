namespace Uma.Eservices.Web.Core.Filters
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Newtonsoft.Json;

    /// <summary>
    /// Filter that intercepts Form submit and validates it before actual MVC HTTP_POST Action
    /// Allows to avoid the need of rehydration of Model properties, if form is invalid/incomplete.
    /// This should be called by AJAX intercepted Form submission.
    /// If Model is valid - ajax gets normal return and form is submitted to its action, 
    /// otherwise AJAXed call should parse returned JSON and apply to fields as validation errors.
    /// More information in this URL
    /// http://timgthomas.com/2013/09/simplify-client-side-validation-by-adding-a-server/
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class FormValidatorAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called before an action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Continue normally if the model is valid.
            if (filterContext == null || filterContext.Controller.ViewData.ModelState.IsValid)
            {
                return;
            }

            var serializationSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            // Serialize Model State for passing back to AJAX call
            var serializedModelState = JsonConvert.SerializeObject(
              filterContext.Controller.ViewData.ModelState,
              serializationSettings);

            var result = new ContentResult
            {
                Content = serializedModelState,
                ContentType = "application/json"
            };

            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            filterContext.Result = result;
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext == null || !filterContext.HttpContext.Request.IsAjaxRequest())
            {
                return;
            }

            // Preparing Json object for AJAX.success processing in forms.js javascript
            string destinationUrl = string.Empty;
            if (filterContext.Result is RedirectResult)
            {
                var result = filterContext.Result as RedirectResult;
                destinationUrl = UrlHelper.GenerateContentUrl(result.Url, filterContext.HttpContext);
            }
                
            if (filterContext.Result is RedirectToRouteResult)
            {
                var result = filterContext.Result as RedirectToRouteResult;
                var helper = new UrlHelper(filterContext.RequestContext);
                destinationUrl = helper.RouteUrl(result.RouteValues);
            }

            // Rendered context is getting reloaded by AJAX.success in forms.js javascript
            if (filterContext.Result is ViewResult)
            {
                return;
            }

            var jsonResult = new JsonResult { Data = new { resultType = "Redirect", redirectUrl = destinationUrl } };
            filterContext.Result = jsonResult;
        }
    }
}