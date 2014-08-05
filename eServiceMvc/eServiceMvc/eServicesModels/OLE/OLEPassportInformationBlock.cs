namespace Uma.Eservices.Models.OLE
{
    using System;
    using System.Collections.Generic;

    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// Block of Passport or similar document data for applicant
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLE")]
    public class OLEPassportInformationBlock : BaseModel
    {
        /// <summary>
        /// Passport type
        /// </summary>
        public string PassportType { get; set; }

        /// <summary>
        /// Diplomatic passport number
        /// </summary>
        public string PassportNumber { get; set; }

        /// <summary>
        /// Passport data is invalid (expired or otherwise)
        /// </summary>
        public bool? InvalidPassport { get; set; }

        /// <summary>
        /// LABEL of STATE (in UMA) of country, which issued this document
        /// </summary>
        public string IssuerCountry { get; set; }

        /// <summary>
        /// Name of issuing authority (document issuer organization/branch)
        /// </summary>
        public string IssuerAuthority { get; set; }

        /// <summary>
        /// Date when document was issued to applicant
        /// </summary>
        public DateTime? IssuedDate { get; set; }

        /// <summary>
        /// Date when document is getting to be expired
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Contains list of Possible passport types
        /// </summary>
        public Dictionary<string, string> PassportTypeList
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "PASSPORT", "Passport" }, { "DIPLOMATIC", "Diplomatic passport" },
                    { "ALIEN", "Alien passport" }, { "REFUGEE", "Refugee travel document" }, 
                    { "OTHER", "Other" }
                };
            }
        }
    }
}
