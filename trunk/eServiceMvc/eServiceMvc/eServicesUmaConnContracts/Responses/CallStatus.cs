namespace Uma.DataConnector.Contracts.Responses
{
    using System.Runtime.Serialization;
    using Uma.DataConnector.Contracts;

    /// <summary>
    /// Enumeration to specify WCF Service call status for clients
    /// </summary>
    [DataContract(Namespace = NS.ServiceNamespaceV1)]
    public enum CallStatus
    {
        /// <summary>
        /// WCF Call is a failure (default) should be expliciltly changed to success if really success
        /// </summary>
        [EnumMember]
        Failed,

        /// <summary>
        /// WCF Service call is a success.
        /// </summary>
        [EnumMember]
        Success
    }
}
