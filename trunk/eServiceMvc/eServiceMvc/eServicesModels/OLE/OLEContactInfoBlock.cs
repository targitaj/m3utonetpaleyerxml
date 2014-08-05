namespace Uma.Eservices.Models.OLE
{
    using System.Collections.Generic;
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// Block of OLE forms Contact information
    /// </summary>
    public class OLEContactInfoBlock : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEContactInfoBlock"/> class.
        /// Creates internal objects and populates defaults
        /// </summary>
        public OLEContactInfoBlock()
        {
            this.AddressInformation = new AddressInformation();
            this.FinlandAddressInformation = new AddressInformation();
            // TODO: Set "FINLAND" as country for Finland address information
        }

        /// <summary>
        /// Address information of applicant
        /// </summary>
        public AddressInformation AddressInformation { get; set; }

        /// <summary>
        /// Telephone number of applicant (mobile!)
        /// </summary>
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// Applicant's e-mail address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Address infomation in Finland, if it differs from one (mandatory) above
        /// </summary>
        public AddressInformation FinlandAddressInformation { get; set; }

        /// <summary>
        /// Telephone number of applicant (mobile!)
        /// </summary>
        public string FinlandTelephoneNumber { get; set; }

        /// <summary>
        /// Applicant's e-mail address
        /// </summary>
        public string FinlandEmailAddress { get; set; }
    }
}
