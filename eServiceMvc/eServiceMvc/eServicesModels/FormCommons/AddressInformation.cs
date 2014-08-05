namespace Uma.Eservices.Models.FormCommons
{
    using System.Diagnostics;

    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// Address information block
    /// </summary>
    [DebuggerDisplay("Id: {Id}")]
    public class AddressInformation : BaseModel
    {
        /// <summary>
        /// Internal ID of an address
        /// </summary>
        public int Id { get; set; }

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
        /// Determines whether two objects are equal (has same property values)
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        public override bool Equals(object obj)
        {
            AddressInformation p = obj as AddressInformation;

            if (p == null)
            {
                return false;
            }

            return (this.City == p.City) &&
                (this.Country == p.Country) &&
                (this.PostalCode == p.PostalCode) &&
                (this.StreetAddress == p.StreetAddress);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
