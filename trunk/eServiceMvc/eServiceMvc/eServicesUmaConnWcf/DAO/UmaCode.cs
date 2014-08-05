namespace Uma.DataConnector.DAO
{
    using System;

    /// <summary>
    /// Class representing schema of [CODE] table in UMA Oracle database
    /// </summary>
    public class UmaCode
    {
        /// <summary>
        /// Database IDentified in Oracle for CODE record
        /// </summary>
        public virtual long CodeId { get; set; }

        /// <summary>
        /// Gets or sets the CODE TYPE, which is grouping several CODEs under one type
        /// This is FK reference to CODE_TYPE table object
        /// </summary>
        public virtual long CodeTypeId { get; set; }

        /// <summary>
        /// Rarely used relation pointer between some CODEs
        /// </summary>
        public virtual long? RelatedCodeId { get; set; }

        /// <summary>
        /// CODE LABEL is human readable ID of particular CODE
        /// </summary>
        public virtual string Label { get; set; }

        /// <summary>
        /// CODE Value in finnish language (always present - mandatory to have)
        /// </summary>
        public virtual string TextFinnish { get; set; }

        /// <summary>
        /// Code value in Swedish - sometimes present, most commonly it is empty/null
        /// </summary>
        public virtual string TextSwedish { get; set; }

        /// <summary>
        /// CODE text in English - not mandatory so can be absent.
        /// </summary>
        public virtual string TextEnglish { get; set; }

        /// <summary>
        /// Some of CODE (Types) can use this for additional functionalities, like TAG or SHORT_NAME or other purposes.
        /// </summary>
        public virtual string CodeValue { get; set; }

        /// <summary>
        /// May contain some descriptive test for CODE
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Contains numerical values to put specific orderin of CODE values within CODE_TYPE group
        /// </summary>
        public virtual int? Ordering { get; set; }

        /// <summary>
        /// Unknown purpose field, which may contain some1-letter value most probably for KELA integration use.
        /// </summary>
        public virtual string KelaValue { get; set; }

        /// <summary>
        /// May contain value of Date/Time when CODE was started to be in use (selectable/choosable)
        /// </summary>
        public virtual DateTime? ValidityStartDate { get; set; }

        /// <summary>
        /// May contain value of Date/Time when CODE was ended to be in use (selectable/choosable)
        /// </summary>
        public virtual DateTime? ValidityEndDate { get; set; }
    }
}