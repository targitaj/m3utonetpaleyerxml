namespace Uma.Eservices.Models.OLE
{
    using System;
    using System.Collections.Generic;
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// Object definitio for person children list
    /// </summary>
    public class OLEChildData : BaseModel
    {
        /// <summary>
        /// Unique Object key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Spouse Person Name(s) - fist name and last name
        /// </summary>
        public PersonName PersonName { get; set; }

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

            if (p == null)
            {
                return false;
            }

            if (this.PersonName == null)
            {
                this.PersonName = new PersonName();
            }

            return (this.Birthday == p.Birthday) &&
                    (this.CurrentCitizenship == p.CurrentCitizenship) &&
                    (this.Gender == p.Gender) &&
                    (this.MigrationIntentions == p.MigrationIntentions) &&
                    (this.PersonCode == p.PersonCode) &&
                    (this.PersonName.Equals(p.PersonName));
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
}
