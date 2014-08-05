namespace Uma.Eservices.Common
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Dependency configuration for Unity Container in Commons space
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DependencyConfig
    {
        /// <summary>
        /// Additionally configures unity container with classes in cross-cutting project
        /// </summary>
        /// <param name="container">Unity container instance</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "ok", Justification = "Required, read comments in code.")]
        public static void RegisterDependencies(IUnityContainer container)
        {
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
