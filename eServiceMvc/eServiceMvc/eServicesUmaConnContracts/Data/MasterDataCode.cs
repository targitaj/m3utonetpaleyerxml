namespace Uma.DataConnector.Contracts.Data
{
    using System;
    using System.Runtime.Serialization;
    using Uma.DataConnector.Contracts;

    /// <summary>
    /// Conains necessary properties of CODE element in UMA database master data portion
    /// </summary>
    [DataContract(Namespace = NS.ServiceNamespaceV1)]
    public class MasterDataCode
    {
        /// <summary>
        /// Database Identifier in Oracle for CODE record
        /// </summary>
        [DataMember]
        public long CodeId { get; set; }

        /// <summary>
        /// CODE LABEL is human readable ID of particular CODE
        /// </summary>
        [DataMember]
        public string Label { get; set; }

        /// <summary>
        /// CODE Value in finnish language (always present - mandatory to have)
        /// </summary>
        [DataMember]
        public string TextFinnish { get; set; }

        /// <summary>
        /// Code value in Swedish - sometimes present, most commonly it is empty/null
        /// </summary>
        [DataMember]
        public string TextSwedish { get; set; }

        /// <summary>
        /// CODE text in English - not mandatory so can be absent.
        /// </summary>
        [DataMember]
        public string TextEnglish { get; set; }

        /// <summary>
        /// Some of CODE (Types) can use this for additional functionalities, like TAG or SHORT_NAME or other purposes.
        /// </summary>
        [DataMember]
        public string CodeValue { get; set; }

        /// <summary>
        /// May contain some descriptive test for CODE
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Contains numerical values to put specific orderin of CODE values within CODE_TYPE group
        /// </summary>
        [DataMember]
        public int? Ordering { get; set; }

        /// <summary>
        /// Unknown purpose field, which may contain some1-letter value most probably for KELA integration use.
        /// </summary>
        [DataMember]
        public string KelaValue { get; set; }

        /// <summary>
        /// May contain value of Date/Time when CODE was started to be in use (selectable/choosable)
        /// </summary>
        [DataMember]
        public DateTime? ValidityStartDate { get; set; }

        /// <summary>
        /// May contain value of Date/Time when CODE was ended to be in use (selectable/choosable)
        /// </summary>
        [DataMember]
        public DateTime? ValidityEndDate { get; set; }
    }
}
