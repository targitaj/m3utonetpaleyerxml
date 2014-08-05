namespace Uma.Eservices.Web.Core
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Route Handler which allows to use underscores in controller names and action names where in reality Underscores are used.
    /// </summary>
    public class HyphenatedRouteHandler : MvcRouteHandler
    {
        /// <summary>
        /// Returns the HTTP handler by using the specified HTTP context.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <returns>
        /// The HTTP handler.
        /// </returns>
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }

            requestContext.RouteData.Values["controller"] =
               requestContext.RouteData.Values["controller"].ToString().Replace("-", "_");
            requestContext.RouteData.Values["action"] =
               requestContext.RouteData.Values["action"].ToString().Replace("-", "_");
            return base.GetHttpHandler(requestContext);
        }
    }
}