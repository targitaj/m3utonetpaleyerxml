namespace Uma.Eservices.DbObjects.FormCommons
{
    using System.Diagnostics;
    using Uma.Eservices.DbObjects.OLE.TableRefEnums;

    /// <summary>
    /// Model to hold person name(s) information
    /// </summary>
    [DebuggerDisplay("Id: {Id}  fName: {FirstName}")]
    public class PersonName
    {
        /// <summary>
        /// Person name object Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// OLEPersonalInformationPage foreign Id
        /// </summary>
        public int OLEPersonalInformationPageId { get; set; }

        /// <summary>
        /// First name(s) of a person
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name(s) of a person
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Get / Set PersonNameRefType enum
        /// </summary>
        public PersonNameRefTypeEnum PersonNameRefType { get; set; }

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

            PersonName p = obj as PersonName;

            return (this.FirstName == p.FirstName) &&
                   (this.LastName == p.LastName) &&
                   (this.PersonNameRefType == p.PersonNameRefType);
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