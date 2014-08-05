namespace Uma.ExternalApi.Contracts.DataContracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// HTTP header added to every WCF call via WCF Service and Client Behavior/Initializer classes
    /// </summary>
    [DataContract(Namespace = NS.ExternalApiNamespaceV1)]
    public class ExternalApiConnectionHeader
    {
        /// <summary>
        /// Transmits Client Culture through WCF call from Client and passes its user CurrentCulture for formatting to service side.
        /// Format of the string is language2-COUNTRY2, like en-US, fi-FI
        /// </summary>
        [DataMember]
        public string ClientCulture { get; set; }

        /// <summary>
        /// Transmits Client UI Culture through WCF call from Client and passes its user CurrentCulture for text translation language.
        /// Format of the string is language2-COUNTRY2, like en-US, fi-FI
        /// </summary>
        [DataMember]
        public string ClientUiCulture { get; set; }
    }
}
