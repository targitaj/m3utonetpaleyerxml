namespace Uma.Eservices.Web
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using FluentValidation.Mvc;
    using Uma.Eservices.Common;
    using Uma.Eservices.Web.Core;
    using Uma.Eservices.Web.Core.Binders;
    using Uma.Eservices.Web.Features.Home;

    /// <summary>
    /// Entry point when MVC Application is started or Session is started
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Method is invoked when Application starts.
        /// Here must reside all top-global initialization routines
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected void Application_Start()
        {
            // Area engine (not used in eServices)
            AreaRegistration.RegisterAllAreas();

            // Routes registration - how request URLs are resolved in application
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Register Globally applied filters (aspects for Controller-Actions)
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // Register CSS styles and JavaScript file bundling and minimification components
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Custom model binder to set Current Thread cultures before anything else
#if DEBUG
            ModelBinders.Binders.DefaultBinder = new CultureAwareModelBinder(new Log("CultureAwareModelBinder"));
#else
            ModelBinders.Binders.DefaultBinder = new CultureAwareModelBinder();
#endif

            // Add specific data types model binders
            ModelBinders.Binders.Add(typeof(DateTime?), new NullableDateTimeBinder());
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeBinder()); 

            // Enable FluentValidations as masta' of all validations through Unity IoC container
            FluentValidationModelValidatorProvider.Configure(x => x.ValidatorFactory = new ModelValidatorFactory());

            // Making Razor Views to be located in "Features" folder together with models and controller for easier management.
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new FeatureConventionViewEngine());
        }

        /// <summary>
        /// Handles the Error event of the Application control.
        /// This is last bastion of unhandled exceptions, which basically intercepts Routing and Binding and View rendering and 404 errors
        /// </summary>
        /// <param name="sender">The source of the event - MVC Application.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data. Not used here.</param>
        [ExcludeFromCodeCoverage]
        protected void Application_Error(object sender, EventArgs e)
        {
            var httpContext = ((MvcApplication)sender).Context;
            var currentController = " ";
            var currentAction = " ";
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null && !string.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null && !string.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
            }

            var ex = Server.GetLastError();
            var controller = new HomeController();
            var routeData = new RouteData();
            var action = MVC.Home.ActionNames.Index;
            Log logger = new Log("MvcApp");
            var httpEx = ex as HttpException;

            if (httpEx != null)
            {
                logger.Error(string.Format(CultureInfo.InvariantCulture, "Unhandled Http {0} exception occured.", httpEx.GetHttpCode()), httpEx);

                switch (httpEx.GetHttpCode())
                {
                    case 404:
                        action = MVC.Home.ActionNames.NotFound;
                        break;

                    // others if any
                }

                httpContext.Response.StatusCode = httpEx.GetHttpCode();
            }
            else
            {
                logger.Error("Unhandled exception occured.", ex);
                httpContext.Response.StatusCode = 500;
            }

            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.TrySkipIisCustomErrors = true;

            routeData.Values["controller"] = MVC.Home.Name;
            routeData.Values["action"] = action;

            HttpContextWrapper wrapper = new HttpContextWrapper(Context);
            var rc = new RequestContext(wrapper, routeData);
            controller.ViewData.Model = new HandleErrorInfo(ex, currentController, currentAction);
            ((IController)controller).Execute(rc);
            Response.End();
        }
    }
}
