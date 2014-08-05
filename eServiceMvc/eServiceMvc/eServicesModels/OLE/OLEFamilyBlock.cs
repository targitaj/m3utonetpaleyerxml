namespace Uma.Eservices.Models.OLE
{
    using System;
    using System.Collections.Generic;
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// Model for OLE forms, containing customer personal data information
    /// </summary>
    public class OLEFamilyBlock : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEPersonalDataBlock"/> class.
        /// Used to initialize empty lists and pre-populate some values (if applicable)
        /// </summary>
        public OLEFamilyBlock()
        {
            this.PreviousNames = new List<PersonName>();
            this.CurrentCitizenships = new List<OLECurrentCitizenship>();
            this.Children = new List<OLEChildData>();
            this.PersonName = new PersonName();
        }

        /// <summary>
        /// Status of Family
        /// </summary>
        public OLEFamilyStatus FamilyStatus { get; set; }

        /// <summary>
        /// Spouse Person Name(s) - fist name and last name
        /// </summary>
        public PersonName PersonName { get; set; }

        /// <summary>
        /// Spouse previous names, that were used somewhere (taken names)
        /// </summary>
        public List<PersonName> PreviousNames { get; set; }

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
        /// LABEL of STATE (in UMA) of Country where person was born
        /// should be something like LATVIA_223
        /// </summary>
        public string BirthCountry { get; set; }

        /// <summary>
        /// Place of person birth (city/town name or something similar)
        /// </summary>
        public string BirthPlace { get; set; }

        /// <summary>
        /// Contains list of LABEL of STATE (in UMA) of a country or countries which citizenship(s) person currently have
        /// </summary>
        public List<OLECurrentCitizenship> CurrentCitizenships { get; set; }

        /// <summary>
        /// To specify spouse intentions or status regarding Finland
        /// </summary>
        public OLEMigrationIntentions SpouseIntentions { get; set; }

        /// <summary>
        /// UI helper to display/hide children data block(s)
        /// </summary>
        public bool HaveChildren { get; set; }

        /// <summary>
        /// List of Children (if any)
        /// </summary>
        public List<OLEChildData> Children { get; set; }

        /// <summary>
        /// Contains logic to determine on what percentage this data block is filled. 
        /// Should return number from 0 to 100 (%)
        /// </summary>
        public int BlockFillPercentage
        {
            get
            {
                // This may be replaced with validator-related logic
                const decimal CountOfRequiredInfoFields = 7;
                int filledFields =
                    (this.PersonName.IsEmpty ? 1 : 0) +
                    (this.Gender != Gender.NotSpecified ? 1 : 0) +
                    (this.Birthday.HasValue ? 1 : 0) +
                    (string.IsNullOrWhiteSpace(this.PersonCode) ? 0 : 1) +
                    (string.IsNullOrWhiteSpace(this.BirthCountry) ? 0 : 1) +
                    (string.IsNullOrWhiteSpace(this.BirthPlace) ? 0 : 1) +
                    (this.CurrentCitizenships != null && this.CurrentCitizenships.Count > 0 ? 1 : 0);
                decimal fillPercentage = filledFields / CountOfRequiredInfoFields * 100;
                return (int)fillPercentage;
            }
        }
    }

    /// <summary>
    /// Enumeration to specify relationship state for person
    /// </summary>
    public enum OLEFamilyStatus : short
    {
        /// <summary>
        /// Default value of enum is unspecified enum value
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
