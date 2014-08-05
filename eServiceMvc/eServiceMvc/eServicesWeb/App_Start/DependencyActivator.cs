namespace Uma.Eservices.Web
{
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity.Mvc;

    /// <summary>
    /// Creates Application level static Container for Dependencies resolution need
    /// Also replaces Default Filter Provider to enable DI in filters
    /// Also creates handler for RequestLifetimeManager
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Dependency Injection configuration activator - defines all components that are getting used by and through DI
        /// </summary>
        public static void DependencyConfiguration()
        {
            // Getting preconfigured Unity Container for entire Application lifecycle (same for all requests)
            var container = DependencyConfig.ContainerInstance;

            // Allows to have [Global] Action Filters (Attributes) which use injected dependency attributes
            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

            // Setting Unity container as Dependency resolver for MVC itself
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            // Providing means (PerRequestLifetimeManager) to have Singleton instances whcih lives only during/inside one Http request 
            // Should be code initialized in other place: http://blog.davidebbo.com/2011/02/register-your-http-modules-at-runtime.html
            // Currently added in Web.Config: Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }
    }
}