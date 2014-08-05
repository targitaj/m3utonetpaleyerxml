namespace Uma.ExternalApi.Contracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enumeration to specify WCF Service call status for clients
    /// </summary>
    [DataContract(Name = "CallStatus", Namespace = NS.ExternalApiNamespaceV1)]
    public enum CallStatus : int
    {
        /// <summary>
        /// WCF Call is a failure (default) should be expliciltly changed to success if really success
        /// </summary>
        [EnumMember(Value = "Failed")]
        Failed,

        /// <summary>
        /// WCF Service call is a success.
        /// </summary>
        [EnumMember(Value = "Success")]
        Success
    }
}
