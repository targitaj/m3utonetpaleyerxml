namespace Uma.DataConnector.Contracts.Responses
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Uma.DataConnector.Contracts;
    using Uma.DataConnector.Contracts.Data;
    using Uma.DataConnector.Contracts.Service;

    /// <summary>
    /// Response object for operation <see cref="IUmaMasterDataService.GetCodeByLabel"/>.
    /// Contains common status and messages plus CODE object if found.
    /// </summary>
    [DataContract(Namespace = NS.ServiceNamespaceV1)]
    public class GetCountryListResponse : ResponseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCountryListResponse"/> class.
        /// Creates empty list of Countries (STATES), so it is not null.
        /// </summary>
        public GetCountryListResponse()
        {
            this.Countries = new List<MasterDataCountry>();
        }

        /// <summary>
        /// List of CODE objects, if any.
        /// Will have empty list when none found
        /// </summary>
        [DataMember]
        public List<MasterDataCountry> Countries { get; set; }
    }
}
