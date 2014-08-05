namespace Uma.Eservices.DbAccess
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Practices.Unity;
    using Uma.Eservices.Common;

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
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerRequestLifetimeManager());
            container.RegisterType<IGeneralDataHelper, GeneralDbDataHelper>();
            container.RegisterType<ILocalizationDataHelper, LocalizationDataHelper>();
        }
    }
}
