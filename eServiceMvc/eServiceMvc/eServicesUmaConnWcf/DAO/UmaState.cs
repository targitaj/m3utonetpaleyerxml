namespace Uma.DataConnector.DAO
{
    using System;

    /// <summary>
    /// UMA databas STATE records object. Describes Countries with their properties
    /// </summary>
    public class UmaState
    {
        /// <summary>
        /// Database Identifier in Oracle for STATE record
        /// </summary>
        public virtual long StateId { get; set; }

        /// <summary>
        /// STATE LABEL is human readable ID of particular STATE (Country)
        /// Example: UKRAINA_209
        /// </summary>
        public virtual string Label { get; set; }

        /// <summary>
        /// STATE name in finnish language (always present - mandatory to have)
        /// </summary>
        public virtual string NameFinnish { get; set; }

        /// <summary>
        /// STATE name in Swedish - not mandatory so can be absent.
        /// </summary>
        public virtual string NameSwedish { get; set; }

        /// <summary>
        /// STATE name in English - not mandatory so can be absent.
        /// </summary>
        public virtual string NameEnglish { get; set; }

        /// <summary>
        /// STATE name in in some native language (not country own) only western++ characters are used - no glyphs, no cyrillics - not mandatory so can be absent.
        /// </summary>
        public virtual string NameNative { get; set; }

        /// <summary>
        /// STATE Border name (whatever it is: mostly the same as NameFinnish) - not mandatory so can be absent.
        /// </summary>
        public virtual string NameBorder { get; set; }

        /// <summary>
        /// Mostly CODE object, describing continent
        /// </summary>
        public virtual UmaCode GreaterArea { get; set; }

        /// <summary>
        /// Optional value - when not null may contain Date when Country (STATE) is not valid for use anymore
        /// </summary>
        public virtual DateTime? ValidityExpired { get; set; }

        /// <summary>
        /// May contain value of Date/Time when STATE was started to be in use (selectable/choosable)
        /// </summary>
        public virtual DateTime? ValidityStartDate { get; set; }

        /// <summary>
        /// May contain value of Date/Time when STATE was ended to be in use (selectable/choosable)
        /// </summary>
        public virtual DateTime? ValidityEndDate { get; set; }
    }
}