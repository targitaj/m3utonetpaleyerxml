namespace Uma.Eservices.DbObjects.FormCommons
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Uma.Eservices.DbObjects.OLE.TableRefEnums;

    /// <summary>
    /// Address information block
    /// </summary>
    [DebuggerDisplay("Id: {Id} type: {AddressInformationRefType}")]
    public class AddressInformation
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
        /// Street address
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// Postal code (a.k.a. ZIP-code)
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Name of the City/Town
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// LABEL of STATE (in UMA) of country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Get / set AddressInformationRefType enum
        /// </summary>
        public OLEAddressInformationRefTypeEnum AddressInformationRefType { get; set; }

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

            AddressInformation p = obj as AddressInformation;

            return (this.AddressInformationRefType == p.AddressInformationRefType) &&
                (this.City == p.City) &&
                (this.Country == p.Country) &&
                (this.PostalCode == p.PostalCode) &&
                (this.StreetAddress == p.StreetAddress);
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
