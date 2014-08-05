namespace Uma.ExternalApi.Contracts.ServiceContracts
{
    using System.ServiceModel;
    using Uma.ExternalApi.Contracts;

    /// <summary>
    /// Write, data submit operations (into UMA) of External API
    /// </summary>
    [ServiceContract(Namespace = NS.ExternalApiNamespaceV1)]
    public interface IExternalApiSet
    {
    }
}
