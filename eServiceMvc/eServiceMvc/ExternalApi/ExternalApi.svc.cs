namespace Uma.ExternalApi
{
    using System;
    using System.ServiceModel;
    using Uma.ExternalApi.Contracts;
    using Uma.ExternalApi.Contracts.ServiceContracts;

    /// <summary>
    /// UMA External API data provider
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, Namespace = NS.ExternalApiNamespaceV1)]
    public class ExternalApi : MarshalByRefObject, IExternalApi
    {
        /// <summary>
        /// Returns current time as string for service testing purposes
        /// </summary>
        /// <returns>Current server time</returns>
        public string GetPingTime()
        {
            return DateTime.Now.ToShortTimeString();
        }
    }
}
