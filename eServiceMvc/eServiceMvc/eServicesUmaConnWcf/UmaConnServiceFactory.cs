namespace Uma.DataConnector
{
    using System.ServiceModel;
    using Microsoft.Practices.Unity;
    using Uma.Eservices.UmaConnWcf;
    using Unity.Wcf;

    /// <summary>
    /// Override of Unity.Wcf Nuget package solution to get configured container
    /// http://unitywcf.codeplex.com/
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class UmaConnServiceFactory : UnityServiceHostFactory
    {
        /// <summary>
        /// Configures the container with Uma style Dependency configurer.
        /// </summary>
        /// <param name="container">The container of da Unity.</param>
        protected override void ConfigureContainer(IUnityContainer container)
        {
            DependencyConfig.RegisterDependencies(container);
        }
    }    
}