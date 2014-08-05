namespace Uma.Eservices.DbObjects.OLE
{
    using System;
    using System.Collections.Generic;
    using Uma.Eservices.DbObjects.FormCommons;
    using Uma.Eservices.DbObjects.OLE.TableRefEnums;

    /// <summary>
    /// Object definitio for person children list
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLE")]
    public class OLEChildData
    {
        /// <summary>
        /// Person name object Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// OLEPersonalInformationPage parent id
        /// </summary>
        public int OLEPersonalInformationPageId { get; set; }

        /// <summary>
        /// Spouse Person FirstName(s)
        /// </summary>
        public string PersonNameFirstName { get; set; }

        /// <summary>
        /// Spouse Person LastName(s)
        /// </summary>
        public string PersonNameLastName { get; set; }

        /// <summary>
        /// Gender (sex) of a spouse
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Date of birth for spouse
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Finnish Person identification (HETU) code
        /// Usually in form of DDMMYY-XXXXX
        /// </summary>
        public string PersonCode { get; set; }

        /// <summary>
        /// Contains LABEL of STATE (in UMA) of a country or countries which citizenship(s) person currently have
        /// </summary>
        public string CurrentCitizenship { get; set; }

        /// <summary>
        /// To specify intentions or status regarding Finland
        /// </summary>
        public OLEMigrationIntentions MigrationIntentions { get; set; }

        /// <summary>
        /// Get / sets OLEChildDataRefType
        /// </summary>
        public OLEChildDataRefTypeEnum OLEChildDataRefType { get; set; }

        /// <summary>
        /// Object Equals overrive. Specific equals class implementation
        /// </summary>
        /// <param name="obj">Object input</param>
        /// <returns>True if this and obj equals</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            OLEChildData p = obj as OLEChildData;

            return (this.OLEChildDataRefType == p.OLEChildDataRefType) &&
                    (this.Birthday == p.Birthday) &&
                    (this.CurrentCitizenship == p.CurrentCitizenship) &&
                    (this.Gender == p.Gender) &&
                    (this.MigrationIntentions == p.MigrationIntentions) &&
                    (this.PersonCode == p.PersonCode) &&
                    (this.PersonNameFirstName == p.PersonNameFirstName) &&
                    (this.PersonNameLastName == p.PersonNameLastName);
        }

        /// <summary>
        /// Method override. Returns Object hashCode
        /// </summary>
        /// <returns>Hash Code of this object</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Enumeration to specify relationship state for person
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLE")]
    public enum OLEFamilyStatus : short
    {
        /// <summary>
        /// Empty / default value
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Default = single (not married)
        /// </summary>
        Single = 1,

        /// <summary>
        /// Married - should be spouse
        /// </summary>
        Married = 2,

        /// <summary>
        /// Person is divoced
        /// </summary>
        Divorced = 3,

        /// <summary>
        /// Person has its spose dead
        /// </summary>
        Widow = 4,

        /// <summary>
        /// Lives together (unregistered relationship)
        /// </summary>
        Cohabitation = 5,

        /// <summary>
        /// Well this is interesting
        /// </summary>
        RegisteredRelationship = 6
    }

    /// <summary>
    /// Enumeration to distinguish spouse intentions regarding Finland
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLE")]
    public enum OLEMigrationIntentions : byte
    {
        /// <summary>
        /// Should be set!
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Is already in Finland, is Finnish citizen
        /// </summary>
        IsInFinland = 1,

        /// <summary>
        /// Is also applying or will apply for Finnish entry
        /// </summary>
        Applying = 2,

        /// <summary>
        /// Not going to move to Finland
        /// </summary>
        WillNotMove = 3
    }
}
