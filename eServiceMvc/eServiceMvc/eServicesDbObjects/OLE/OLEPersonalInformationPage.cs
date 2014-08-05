namespace Uma.Eservices.DbObjects.OLE
{
    using System;
    using System.Collections.Generic;
    using Uma.Eservices.DbObjects.FormCommons;

    /// <summary>
    /// Common for most (all) OLE forms - personam (Customer) general information
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLE")]
    public class OLEPersonalInformationPage
    {
        /// <summary>
        /// Model default ctor. Init all ref object in model
        /// </summary>
        public OLEPersonalInformationPage()
        {
            this.OleAddressInformationList = new List<AddressInformation>();
            this.OleChildDataList = new List<OLEChildData>();
            this.OlePersonNameList = new List<PersonName>();
            this.OleCitizenShipList = new List<OLECitizenship>();
        }

        /// <summary>
        /// OLEPersonalInformationPage object Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Contains ID valuue of Application to be preserved between web calls
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Gets / sets Person FirstName
        /// </summary>
        public string PersonalPersonNameFirstName { get; set; }

        /// <summary>
        /// Gets / sets Person LastName
        /// </summary>
        public string PersonalPersonNameLastName { get; set; }

        /// <summary>
        /// Gets / sets Person Gender
        /// </summary>
        public Gender PersonalGender { get; set; }

        /// <summary>
        /// Gets / sets Person BirthDay
        /// </summary>
        public DateTime? PersonalBirthday { get; set; }

        /// <summary>
        /// Gets / sets Person Code
        /// </summary>
        public string PersonalPersonCode { get; set; }

        /// <summary>
        /// Gets / sets Person Birth Country
        /// </summary>
        public string PersonalBirthCountry { get; set; }

        /// <summary>
        /// Gets / sets Person Birth Place
        /// </summary>
        public string PersonalBirthPlace { get; set; }

        /// <summary>
        /// Gets / sets Person Mother Language
        /// </summary>
        public string PersonalMotherLanguage { get; set; }

        /// <summary>
        /// Gets / sets Person Communitation language
        /// </summary>
        public CommunicationLanguage PersonalCommunicationLanguage { get; set; }

        /// <summary>
        /// Gets / sets Person Occupation
        /// </summary>
        public string PersonalOccupation { get; set; }

        /// <summary>
        /// Gets / sets Person Education
        /// </summary>
        public string PersonalEducation { get; set; }

        /// <summary>
        /// Gets / sets Copntact Telephone number
        /// </summary>
        public string ContactTelephoneNumber { get; set; }

        /// <summary>
        /// Gets / sets Copntact Email address
        /// </summary>
        public string ContactEmailAddress { get; set; }

        /// <summary>
        /// Gets / sets Copntact Finland Telephone number
        /// </summary>
        public string ContactFinlandTelephoneNumber { get; set; }

        /// <summary>
        /// Gets / sets Copntact Finland eMail address number
        /// </summary>
        public string ContactFinlandEmailAddress { get; set; }

        /// <summary>
        /// Gets / sets Passport type
        /// </summary>
        public string PassportPassportType { get; set; }

        /// <summary>
        /// Gets / sets Passport number
        /// </summary>
        public string PassportPassportNumber { get; set; }

        /// <summary>
        /// Gets / sets is Passport valid
        /// </summary>
        public bool? PassportInvalidPassport { get; set; }

        /// <summary>
        /// Gets / sets Passport Issuer Country
        /// </summary>
        public string PassportIssuerCountry { get; set; }

        /// <summary>
        /// Gets / sets Passport Issuer Authority
        /// </summary>
        public string PassportIssuerAuthority { get; set; }

        /// <summary>
        /// Gets / sets Passport Issue date
        /// </summary>
        public DateTime? PassportIssuedDate { get; set; }

        /// <summary>
        /// Gets / sets Passport Expiration Date
        /// </summary>
        public DateTime? PassportExpirationDate { get; set; }

        /// <summary>
        /// Gets / sets Residence Is already in finland
        /// </summary>
        public bool? ResidenceDurationAlreadyInFinland { get; set; }

        /// <summary>
        /// Gets / sets Residence Durration Arrival date
        /// </summary>
        public DateTime? ResidenceDurationArrivalDate { get; set; }

        /// <summary>
        /// Gets / sets Residence Duration Depart date
        /// </summary>
        public DateTime? ResidenceDurationDepartDate { get; set; }

        /// <summary>
        /// Gets / sets Residence Duration reason
        /// </summary>
        public string ResidenceDurationDurationOfStay { get; set; }

        /// <summary>
        /// Gets / sets Family status
        /// </summary>
        public OLEFamilyStatus FamilyStatus { get; set; }

        /// <summary>
        /// Gets / sets Family FirstName
        /// </summary>
        public string FamilyPersonNameFirstName { get; set; }

        /// <summary>
        /// Gets / sets Family LastName
        /// </summary>
        public string FamilyPersonNameLastName { get; set; }

        /// <summary>
        /// Gets / sets Family person Fender
        /// </summary>
        public Gender FamilyGender { get; set; }

        /// <summary>
        /// Gets / sets Family perosn birth day
        /// </summary>
        public DateTime? FamilyBirthday { get; set; }

        /// <summary>
        /// Gets / sets Family person code
        /// </summary>
        public string FamilyPersonCode { get; set; }

        /// <summary>
        /// Gets / sets Family person birth country
        /// </summary>
        public string FamilyBirthCountry { get; set; }

        /// <summary>
        /// Gets / sets Family person birth place
        /// </summary>
        public string FamilyBirthPlace { get; set; }

        /// <summary>
        /// Gets / sets Family person occupation
        /// </summary>
        public OLEMigrationIntentions FamilySpouseIntentions { get; set; }

        /// <summary>
        /// Gets / sets Do Family person have children
        /// </summary>
        public bool FamilyHaveChildren { get; set; }

        // OLE list properties

        /// <summary>
        /// Object citizenShip list
        /// </summary>
        public virtual List<OLECitizenship> OleCitizenShipList { get; set; }

        /// <summary>
        /// Object peerson name list
        /// </summary>
        public virtual List<PersonName> OlePersonNameList { get; set; }

        /// <summary>
        /// Object Address info list
        /// </summary>
        public virtual List<AddressInformation> OleAddressInformationList { get; set; }

        /// <summary>
        /// Object Child data list
        /// </summary>
        public virtual List<OLEChildData> OleChildDataList { get; set; }
    }
}
