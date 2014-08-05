namespace Uma.ExternalApi.Contracts.ServiceContracts
{
    using System.ServiceModel;
    using Uma.ExternalApi.Contracts;

    /// <summary>
    /// Read / data retrieval (from UMA) operations for External API
    /// </summary>
    [ServiceContract(Namespace = NS.ExternalApiNamespaceV1)]
    public interface IExternalApiGet
    {
        /// <summary>
        /// Provide simple service method to assess whether service works at all. 
        /// Returns current server time just for demo/test purposes.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Its WCF method")]
        [OperationContract]
        string GetPingTime();
    }
}
