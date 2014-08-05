namespace Uma.DataConnector.Contracts.Responses
{
    using System.Runtime.Serialization;
    using Uma.DataConnector.Contracts;
    using Uma.DataConnector.Contracts.Data;
    using Uma.DataConnector.Contracts.Service;

    /// <summary>
    /// Response object for operation <see cref="IUmaMasterDataService.GetCodeByLabel"/> and <see cref="IUmaMasterDataService.GetCodeById"/>.
    /// Contains common status and messages plus CODE object if found.
    /// </summary>
    [DataContract(Namespace = NS.ServiceNamespaceV1)]
    public class GetCodeResponse : ResponseBase
    {
        /// <summary>
        /// Code object, if found
        /// </summary>
        [DataMember]
        public MasterDataCode Code { get; set; }
    }
}
