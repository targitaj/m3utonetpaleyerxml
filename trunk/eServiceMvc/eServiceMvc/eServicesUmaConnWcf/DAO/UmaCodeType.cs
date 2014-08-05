namespace Uma.DataConnector.DAO
{
    using System.Collections.Generic;

    /// <summary>
    /// Counterpart of a record in UMA table CODE_TYPE.
    /// It is grouping element for CODE(s) 
    /// </summary>
    public class UmaCodeType
    {
        /// <summary>
        /// Code type identifier in database for record which represents this object instance.
        /// </summary>
        public virtual long CodeTypeId { get; set; }

        /// <summary>
        /// May contain <see cref="CodeTypeId"/> of <see cref="UmaCodeType"/> which is grouping element for several Code types
        /// </summary>
        public virtual long? CodeTypeGroupId { get; set; }

        /// <summary>
        /// Human readable Identifier for Code Type
        /// </summary>
        public virtual string Label { get; set; }

        /// <summary>
        /// Purpose is to have Name of a Code Type element, however it may contain LABEL duplicate
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Rarely contains details on what this code type is
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this code type is a grouping type for others.
        /// </summary>
        public virtual bool? IsGroup { get; set; }

        /// <summary>
        /// Collection of Codes that goes under this code type
        /// </summary>
        public virtual IList<UmaCode> Codes { get; set; }
    }
}