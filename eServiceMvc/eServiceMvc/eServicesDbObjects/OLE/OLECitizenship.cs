namespace Uma.Eservices.DbObjects.OLE
{
    using System.Collections.Generic;
    using Uma.Eservices.DbObjects.OLE.TableRefEnums;

    /// <summary>
    /// OLECitizenship class
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLE")]
    public class OLECitizenship
    {
        /// <summary>
        /// OLECitizenship id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// OLEPersonalInformationPageId foreign id
        /// </summary>
        public int OLEPersonalInformationPageId { get; set; }

        /// <summary>
        /// Get / sets Citizenship
        /// </summary>
        public string Citizenship { get; set; }

        /// <summary>
        /// Get / sets CitizenshipRefType
        /// </summary>
        public OLECitizenshipRefTypeEnum CitizenshipRefType { get; set; }

        /// <summary>
        /// Object Equals overrive. Specific equals class implementation
        /// </summary>
        /// <param name="obj">Object input</param>
        /// <returns>True if this and obj equals</returns>
        public override bool Equals(object obj)
        {
            OLECitizenship p = obj as OLECitizenship;

            if (p == null)
            {
                return false;
            }

            return (this.CitizenshipRefType == p.CitizenshipRefType) &&
                (this.Citizenship == p.Citizenship);
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
