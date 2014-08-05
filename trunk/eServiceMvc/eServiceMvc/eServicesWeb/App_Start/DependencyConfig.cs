namespace Uma.Eservices.Web
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;

    using Microsoft.Owin.Security;
    using Microsoft.Practices.Unity;

    using Uma.Eservices.Web.Components.PdfViewCreator;
    using Uma.Eservices.Web.Core.Filters;

    /// <summary>
    /// Class to register Unity IoC container class dependencies
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DependencyConfig
    {
        /// <summary>
        /// Gets the Unity container instance if it required for any other components as property or attribute
        /// </summary>
        public static IUnityContainer ContainerInstance
        {
            get
            {
                return internalLazyContainer.Value;
            }
        }

        /// <summary>
        /// The container instance as static variable, lazily instantiated
        /// </summary>
        private static Lazy<IUnityContainer> internalLazyContainer = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterComponents(container);
            return container;
        });

        /// <summary>
        /// Registers the components with Unity container
        /// </summary>
        /// <param name="container">The Unity container.</param>
        public static void RegisterComponents(IUnityContainer container)
        {
            // Adds configuration of Cross-Cutting concerns project lib 
            // Do it as first, to make sure all logging, exceptions etc. goes first.
            // Logic has access to all other components, there next chain DepConfigs are called
            Common.DependencyConfig.RegisterDependencies(container);
            Logic.DependencyConfig.RegisterDependencies(container);

            // Here goes Web classes registrations
            // Controller classes is no need to register
            container.RegisterType<IExceptionFilter, ControllerErrorHandlerFilterAttribute>("exceptionFilter");
            container.RegisterType<IActionFilter, CallContextSaveFilterAttribute>("contextSave");
            container.RegisterType<IActionFilter, FeatureNameFilterAttribute>("featureNameSave");
            container.RegisterType<IActionFilter, LocalizationFilter>("cultureFilter");

            container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            container.RegisterType<IPdfCreator, PdfCreator>();
        }
    }
}