namespace Uma.DataConnector
{
    using System.Diagnostics.CodeAnalysis;
    using System.ServiceModel.Description;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using NHibernate;
    using Uma.DataConnector.Contracts.Service;
    using Uma.DataConnector.Logging;

    /// <summary>
    /// Dependency configuration for Unity Container in DbAccess space
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DependencyConfig
    {
        /// <summary>
        /// Additionally configures unity container with classes in Database access project
        /// </summary>
        /// <param name="container">Unity container instance</param>
        public static void RegisterDependencies(IUnityContainer container)
        {
            container.RegisterType<IServiceBehavior, UmaConnNHibernateBehavior>("nHibernate");

            container.RegisterInstance<ISessionFactory>(UmaConnNhibernateFactory.Instance, new ContainerControlledLifetimeManager());
            container.RegisterType<ISession>(
                new HierarchicalLifetimeManager(),
                new InjectionFactory(c => container.Resolve<ISessionFactory>().OpenSession()));

            container.RegisterType<IUmaMasterDataService, UmaMasterDataService>();
            container.RegisterType<ICallHandler, UmaConnTransactionHandler>("TransactionHandler");

            container.AddNewExtension<Interception>();
            container.Configure<Interception>().SetInterceptorFor<UmaMasterDataService>(new TransparentProxyInterceptor());

            // These extensions will handle cases where 
            // ILog is resolved as Property with [Dependency] attribute.
            // Will create Logger with name of resolving Type.
            // http://blog.baltrinic.com/software-development/dotnet/log4net-integration-with-unity-ioc-container
            container
                .AddNewExtension<BuildTracking>()
                .AddNewExtension<LogCreation>();

            // DO NOT DELETE THIS CODE UNLESS WE NO LONGER REQUIRE ASSEMBLY NLOG.EXTENDED!!!
            var dummyCall = typeof(NLog.Web.NLogHttpModule);
            var ok = dummyCall.Assembly.CodeBase;
        }
    }
}