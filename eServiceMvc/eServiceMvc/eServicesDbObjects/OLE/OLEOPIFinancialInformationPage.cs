namespace Uma.Eservices.DbObjects.OLE
{
    using System;

    /// <summary>
    /// OLEOPIFinancialInformationPage db model
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLEOPI")]
    public class OLEOPIFinancialInformationPage
    {
        /// <summary>
        /// OLEOPIFinancialInformationPage object Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Contains ID valuue of Application to be preserved between web calls
        /// </summary>
        public int ApplicationId { get; set; }

        #region Financial block properties

        /// <summary>
        /// Enumerated value to specify income/financials to support education in Finland
        /// </summary>
        public OLEOPISupportTypes FinancialIncomeInfo { get; set; }

        /// <summary>
        /// When "Other" is chosen in <see cref="IncomeInfo"/>, here should go description
        /// </summary>
        public string FinancialOtherIncome { get; set; }

        /// <summary>
        /// Whether applicant is already studying somewhere
        /// </summary>
        public bool? FinancialIsCurrentlyStudying { get; set; }

        /// <summary>
        /// Is applicant currently working somewhere
        /// </summary>
        public bool? FinancialIsCurrentlyWorking { get; set; }

        /// <summary>
        /// Name of a pleace where he does studies or working
        /// </summary>
        public string FinancialStudyWorkplaceName { get; set; }

        #endregion

        #region HealthInsurance block properties

        /// <summary>
        /// Makes selection that he studies two or more years and it is covered by insurance
        /// </summary>
        public bool? HealthInsuredForAtLeastTwoYears { get; set; }

        /// <summary>
        /// Makes selection that he studies up to two years and has appropriate insurance
        /// </summary>
        public bool? HealthInsuredForLessThanTwoYears { get; set; }

        /// <summary>
        /// Whether applicant has KELA social insurance card
        /// </summary>
        public bool? HealthHaveKelaCard { get; set; }

        /// <summary>
        /// Flag whether applicant has EU Healt insurance card
        /// </summary>
        public bool? HealthHaveEuropeanHealtInsurance { get; set; }

        #endregion

        #region Additional info block properties

        /// <summary>
        /// Additional informaion free-text field
        /// </summary>
        public string AdditionalInformation { get; set; }

        #endregion

        #region Criminal info  block properties

        /// <summary>
        /// Flag - does applicant had conviction / sentenced to punishment in crime
        /// </summary>
        public bool? CriminalHaveCrimeConviction { get; set; }

        /// <summary>
        /// Description of conviction in crime
        /// </summary>
        public string CriminalConvictionCrimeDescription { get; set; }

        /// <summary>
        /// Country LABLE of UMA master data where crime conviction took place
        /// </summary>
        public string CriminalConvictionCountry { get; set; }

        /// <summary>
        /// Date of Crime Conviction
        /// </summary>
        public DateTime? CriminalConvictionDate { get; set; }

        /// <summary>
        /// Description of what was sentence in crime
        /// </summary>
        public string CriminalConvictionSentence { get; set; }

        /// <summary>
        /// Flag whether user was suspect of crime
        /// </summary>
        public bool? CriminalWasSuspectOfCrime { get; set; }

        /// <summary>
        /// Description of wat crime applicant was in suspect
        /// </summary>
        public string CriminalCrimeAllegedOffence { get; set; }

        /// <summary>
        /// Country LABEL of UMA master data - where suspiction in crime took place
        /// </summary>
        public string CriminalCrimeCountry { get; set; }

        /// <summary>
        /// Date when suspiction in crime took place
        /// </summary>
        public DateTime? CriminalCrimeDate { get; set; }

        /// <summary>
        /// Flag whether user is OK to retrive crime records out of abroad for validation
        /// </summary>
        public bool? CriminalRecordApproval { get; set; }

        /// <summary>
        /// If applicand denies of crime record retrival in <see cref="CriminalRecordApproval"/> - why
        /// </summary>
        public string CriminalRecordRetriveDenialReason { get; set; }

        /// <summary>
        /// Flag whether applicant was denied to enter Schengen zone
        /// </summary>
        public bool? CriminalWasSchengenEntryRefusal { get; set; }

        /// <summary>
        /// What country (LABEL in UMA) applicant was denied entering Schengen zone
        /// </summary>
        public string CriminalSchengenEntryRefusalCountry { get; set; }

        /// <summary>
        /// Flag on whether applicant is still refused to enter Schengen zone
        /// </summary>
        public bool? CriminalIsSchengenZoneEntryStillInForce { get; set; }

        /// <summary>
        /// When Schengen zone entry expired or what time it will expire, correlates with <see cref="IsSchengenZoneEntryStillInForce"/>
        /// </summary>
        public DateTime? CriminalSchengenEntryTimeRefusalExpiration { get; set; }

        #endregion
    }
}
