namespace Uma.Eservices.Models.OLE
{
    using System;
    using System.Collections.Generic;
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// Model for OLE forms, containing customer personal data information
    /// </summary>
    public class OLEPersonalDataBlock : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEPersonalDataBlock"/> class.
        /// Used to initialize empty lists and pre-populate some values (if applicable)
        /// </summary>
        public OLEPersonalDataBlock()
        {
            this.PreviousNames = new List<PersonName>();
            this.CurrentCitizenships = new List<OLECurrentCitizenship>();
            this.PreviousCitizenships = new List<OLEPreviousCitizenship>();
        }

        /// <summary>
        /// Person Name(s) - fist name and last name
        /// </summary>
        public PersonName PersonName { get; set; }

        /// <summary>
        /// Person previous names, that were used somewhere (taken names)
        /// </summary>
        public List<PersonName> PreviousNames { get; set; }

        /// <summary>
        /// Gender (sex) of a person
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Date of birth
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
        /// Contains list of LABEL of STATE (in UMA) of countries which citizenship(s) person had in a past
        /// </summary>
        public List<OLEPreviousCitizenship> PreviousCitizenships { get; set; }

        /// <summary>
        /// Contains LABEL of LANGUAGES (in UMA) of language which is native tongue for person
        /// </summary>
        public string MotherLanguage { get; set; }

        /// <summary>
        /// Communication Language selection to one of supported communication languages.
        /// </summary>
        public CommunicationLanguage CommunicationLanguage { get; set; }

        /// <summary>
        /// Professional occupation of a person (what he does for living)
        /// </summary>
        public string Occupation { get; set; }

        /// <summary>
        /// Level of education - one of predefined values
        /// </summary>
        public string Education { get; set; }
    }
}