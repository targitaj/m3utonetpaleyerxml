namespace Uma.Eservices.DbObjects
{
    using System;

    /// <summary>
    /// OLE Forms data 
    /// </summary>
    public class FormDataOLE
    {
        /// <summary>
        /// Form OLE Data portion IDentifier in database
        /// </summary>
        public virtual int FormDataOLEId { get; set; }

        /// <summary>
        /// Referenced main form data object
        /// </summary>
        public ApplicationForm ApplicationFormData { get; set; }

        /// <summary>
        /// Last name of a person
        /// </summary>
        public virtual string PersLastName { get; set; }

        /// <summary>
        /// First name aof a person
        /// </summary>
        public virtual string PersFirstName { get; set; }

        /// <summary>
        /// Gender of a person - Enum numerical representation, 1-male, 2-female
        /// </summary>
        public virtual byte? PersGender { get; set; }

        /// <summary>
        /// Birthdate of a person
        /// </summary>
        public DateTime? PersBirthday { get; set; }

        /// <summary>
        /// Identity code (HETU) for person
        /// </summary>
        public string PersIdentityCode { get; set; }

        /// <summary>
        /// LABEL of STATE in UMA for person country of birth
        /// </summary>
        public string PersBirthCountry { get; set; }

        /// <summary>
        /// Person birth place name
        /// </summary>
        public string PersBirthPlace { get; set; }

        /// <summary>
        /// LABEL of CODE for languages data in UMA - mother language of a person
        /// </summary>
        public string PersMotherLanguage { get; set; }

        /// <summary>
        /// Enumeration representation of preferred language of communication
        /// 0=En, 1=FI, 2=SV
        /// </summary>
        public byte PersPreferredLanguage { get; set; }

        /// <summary>
        /// Person level of education
        /// </summary>
        public string PersEducationLevel { get; set; }

        /// <summary>
        /// Professional occupation of a person 
        /// </summary>
        public string PersProfessionalOccupation { get; set; }
    }
}
