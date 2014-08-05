namespace Uma.ExternalApi.Contracts.ServiceContracts
{
    using System.ServiceModel;
    using Uma.ExternalApi.Contracts;

    /// <summary>
    /// Cumulative External API interface of all GET and SET operations
    /// Use this to implement actual service
    /// </summary>
    [ServiceContract(Namespace = NS.ExternalApiNamespaceV1)]
    public interface IExternalApi : IExternalApiGet, IExternalApiSet
    {
        // Add data read (GET) operations in *Get interface
        // Add data write (SET) operation sin *Set interface
    }
}
