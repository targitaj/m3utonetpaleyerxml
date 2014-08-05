namespace Uma.Eservices.UmaConnClient
{
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Dependency configuration for Unity container in UMA Connector project
    /// </summary>
    public static class DependencyConfig
    {
        /// <summary>
        /// Additionally configures unity container with classes in UMA Connector Client project
        /// </summary>
        /// <param name="container">Unity container instance</param>
        public static void RegisterDependencies(IUnityContainer container)
        {
            // WCF Service Client registrations (ChannelFactory and Channel)
            // Do not use those directly in code! Use IUmaConnProxy implementations instead
            //container.RegisterWcfClient<IUmaMasterDataService>();

            // Channel wrapper regirtrations (these must be actually used in Logic code)
            //container.RegisterType<IUmaConnProxy<IUmaMasterDataService>, UmaConnProxy<IUmaMasterDataService>>();
        }
    }
}
