namespace Uma.Eservices.UmaConnClient
{
    using System.ServiceModel;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Extension method for Unity container to register WCF Service client functionalities through shared WCF Service Interface
    /// </summary>
    public static class UnityExtensions
    {
        /// <summary>
        /// Registers a WCF Service Client objects through shared WCF Service interface
        /// It registers both ChannelFactory (heavy object) as Singleton (default)
        /// and also Channel itself.
        /// To use - resolve Service Interface <typeparam name="TSvc">{T}</typeparam> and you'll get Channel.
        /// </summary>
        /// <typeparam name="TSvc">The Interface the WCF Service Implements</typeparam>
        /// <param name="container">The Unity container within application.</param>
        /// <param name="lm">The Lifetime manager to use for WCF Channel Factory creation. If NULL (default) - ContainerControlledLifetimeManager (Singleton) is used.</param>
        /// <param name="serviceEndpointUrl">The service endpoint URL.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#", Justification = "String is OK for URLs here")]
        public static void RegisterWcfClient<TSvc>(this IUnityContainer container, LifetimeManager lm = null, string serviceEndpointUrl = null)
        {
            if (lm == null)
            {
                lm = new ContainerControlledLifetimeManager();
            }

            container.RegisterType<ChannelFactory<TSvc>>(lm, new InjectionFactory(c => WcfUtility.GetChannelFactory<TSvc>(serviceEndpointUrl)));
            container.RegisterType<TSvc>(new InjectionFactory(c => c.Resolve<ChannelFactory<TSvc>>().CreateChannel()));
        }
    }
}
