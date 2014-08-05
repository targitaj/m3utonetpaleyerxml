namespace Uma.DataConnector.Contracts.Responses
{
    using System.Runtime.Serialization;
    using Uma.DataConnector.Contracts.Data;
    using Uma.DataConnector.Contracts.Service;

    /// <summary>
    /// Response object for operation <see cref="IUmaMasterDataService.GetCountryById"/>.
    /// Contains common status and messages plus STATE object if found.
    /// </summary>
    [DataContract(Namespace = NS.ServiceNamespaceV1)]
    public class GetCountryResponse : ResponseBase
    {
        /// <summary>
        /// Country (STATE) object, if found
        /// </summary>
        [DataMember]
        public MasterDataCountry Country { get; set; }
    }
}
