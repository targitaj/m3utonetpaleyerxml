namespace Uma.Eservices.Web
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Uma.Eservices.Web.Core;

    /// <summary>
    /// Routes configuration class - URL "treatment"
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class RouteConfig
    {
        /// <summary>
        /// Registers the routes for eServices application
        /// This is called from initialization point - Global.asax
        /// </summary>
        /// <param name="routes">The routes collection.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            // Allows to use Attribute Routes
            routes.MapMvcAttributeRoutes();

            routes.Add(
                "LocaleRoute",
                new Route(
                    "{lang}/{controller}/{action}/{id}",
                    new RouteValueDictionary(new { controller = "Home", action = "Index", area = string.Empty, id = UrlParameter.Optional, lang = UrlParameter.Optional }),
                    new RouteValueDictionary { { "lang", @"^[A-Za-z]{2}(-[A-Za-z]{2})*$" } },
                    new HyphenatedRouteHandler()));

            routes.Add(
                "Default",
                new Route(
                    "{controller}/{action}/{id}",
                    new RouteValueDictionary(new { controller = "Home", action = "Index", area = string.Empty, id = UrlParameter.Optional }),
                    new HyphenatedRouteHandler()));
        }
    }
}
