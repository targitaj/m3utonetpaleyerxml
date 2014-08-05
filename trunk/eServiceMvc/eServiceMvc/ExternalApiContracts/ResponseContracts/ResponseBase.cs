namespace Uma.ExternalApi.Contracts.ResponseContracts
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Uma.ExternalApi.Contracts;

    /// <summary>
    /// Response base class to include common propoerties for all WCf Service call responses.
    /// All Response classes must inherit this class to add its properties
    /// </summary>
    [DataContract(Namespace = NS.ExternalApiNamespaceV1)]
    public class ResponseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseBase"/> class.
        /// </summary>
        public ResponseBase()
        {
            this.OperationCallMessages = new List<string>();
        }

        /// <summary>
        /// Flag to determine WCF Call status. It takes default value of FAILED, so it should be explicitly set to Success.
        /// </summary>
        [DataMember(Name = "OperationCallStatus", IsRequired = true)]
        public CallStatus OperationCallStatus { get; set; }

        /// <summary>
        /// May contain error or status messages related to WCF Operation call.
        /// If <see cref="OperationCallStatus"/> is a failure this should contain error messages which happened in service call
        /// </summary>
        [DataMember(Name = "OperationCallMessages")]
        public List<string> OperationCallMessages { get; set; }
    }
}
