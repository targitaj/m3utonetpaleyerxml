namespace Uma.Eservices.VetumaConn
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Dependency configuration for Unity container in UMA Vetuma project
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class DependencyConfig
    {
        /// <summary>
        /// Additionally configures unity container with classes in UMA Vetuma project
        /// </summary>
        /// <param name="container">Unity container instance</param>
        public static void RegisterDependencies(IUnityContainer container)
        {
            container.RegisterType<IStrongAuthenticationService, StrongAuthenticationService>();
            container.RegisterType<IPaymentService, PaymentService>();
            container.RegisterType<IVetumaService, VetumaServiceSubmit>();
            container.RegisterType<IVetumaUtilities, VetumaUtilities>();
        }
    }
}
