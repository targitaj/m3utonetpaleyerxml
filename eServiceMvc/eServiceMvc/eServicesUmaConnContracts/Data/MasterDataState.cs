namespace Uma.DataConnector.Contracts.Data
{
    using System;
    using System.Runtime.Serialization;
    using Uma.DataConnector.Contracts;

    /// <summary>
    /// Conains necessary properties of STATE element in UMA database master data portion
    /// </summary>
    [DataContract(Namespace = NS.ServiceNamespaceV1)]
    public class MasterDataCountry
    {
        /// <summary>
        /// Database Identifier in Oracle for STATE record
        /// </summary>
        [DataMember]
        public long StateId { get; set; }

        /// <summary>
        /// STATE LABEL is human readable ID of particular STATE (Country)
        /// Example: UKRAINA_209
        /// </summary>
        [DataMember]
        public string Label { get; set; }

        /// <summary>
        /// STATE name in finnish language (always present - mandatory to have)
        /// </summary>
        [DataMember]
        public string NameFinnish { get; set; }

        /// <summary>
        /// STATE name in Swedish - not mandatory so can be absent.
        /// </summary>
        [DataMember]
        public string NameSwedish { get; set; }

        /// <summary>
        /// STATE name in English - not mandatory so can be absent.
        /// </summary>
        [DataMember]
        public string NameEnglish { get; set; }

        /// <summary>
        /// STATE name in in some native language (not country own) only western++ characters are used - no glyphs, no cyrillics - not mandatory so can be absent.
        /// </summary>
        [DataMember]
        public string NameNative { get; set; }

        /// <summary>
        /// STATE Border name (whatever it is: mostly the same as NameFinnish) - not mandatory so can be absent.
        /// </summary>
        [DataMember]
        public string NameBorder { get; set; }

        /// <summary>
        /// Mostly CODE object, describing continent
        /// </summary>
        [DataMember]
        public MasterDataCode GreaterArea { get; set; }

        /// <summary>
        /// Optional value - when not null may contain Date when Country (STATE) is not valid for use anymore
        /// </summary>
        [DataMember]
        public DateTime? ValidityExpired { get; set; }

        /// <summary>
        /// May contain value of Date/Time when STATE was started to be in use (selectable/choosable)
        /// </summary>
        [DataMember]
        public DateTime? ValidityStartDate { get; set; }

        /// <summary>
        /// May contain value of Date/Time when STATE was ended to be in use (selectable/choosable)
        /// </summary>
        [DataMember]
        public DateTime? ValidityEndDate { get; set; }
    }
}
