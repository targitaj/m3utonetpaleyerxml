namespace Uma.Eservices.Web
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using Uma.Eservices.Web.Core.Filters;

    /// <summary>
    /// Global MVC Filter configuration file
    /// </summary>
    public static class FilterConfig
    {
        /// <summary>
        /// Registers the global filters.
        /// </summary>
        /// <param name="filters">The filters collection.</param>
        [ExcludeFromCodeCoverage]
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            if (filters == null)
            {
                return;
            }

            filters.Add(DependencyConfig.ContainerInstance.Resolve<IExceptionFilter>("exceptionFilter"), 0);
            filters.Add(DependencyConfig.ContainerInstance.Resolve<IActionFilter>("featureNameSave"), 1);
            filters.Add(DependencyConfig.ContainerInstance.Resolve<IActionFilter>("cultureFilter"), 2);
            filters.Add(DependencyConfig.ContainerInstance.Resolve<IActionFilter>("contextSave"), 3);
            filters.Add(new AuthorizeAttribute(), 4);
            filters.Add(new RequireHttpsAttribute(), 5);
        }
    }
}
