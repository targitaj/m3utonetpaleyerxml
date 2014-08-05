namespace Uma.Eservices.Models.OLE
{
    /// <summary>
    /// Income info to cover studies
    /// First block in Finance/Criminal page
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLEOPI")]
    public class OLEOPIFinancialSupportBlock
    {
        /// <summary>
        /// Enumerated value to specify income/financials to support education in Finland
        /// </summary>
        public OLEOPISupportTypes IncomeInfo { get; set; }

        /// <summary>
        /// When "Other" is chosen in <see cref="IncomeInfo"/>, here should go description
        /// </summary>
        public string OtherIncome { get; set; }

        /// <summary>
        /// Whether applicant is already studying somewhere
        /// </summary>
        public bool? IsCurrentlyStudying { get; set; }

        /// <summary>
        /// Is applicant currently working somewhere
        /// </summary>
        public bool? IsCurrentlyWorking { get; set; }

        /// <summary>
        /// Name of a pleace where he does studies or working
        /// </summary>
        public string StudyWorkplaceName { get; set; }

        /// <summary>
        /// Contains logic to determine on what percentage this data block is filled. 
        /// Should return number from 0 to 100 (%)
        /// </summary>
        public int BlockFillPercentage
        {
            get
            {
                // This may be replaced with validator-related logic
                const decimal CountOfRequiredInfoFields = 4;
                int filledFields =
                    (this.IncomeInfo != OLEOPISupportTypes.Unspecified ? 1 : 0) +
                    (this.IsCurrentlyStudying.HasValue ? 1 : 0) +
                    (this.IsCurrentlyWorking.HasValue ? 1 : 0) +
                    (string.IsNullOrWhiteSpace(this.StudyWorkplaceName) ? 0 : 1);
                decimal fillPercentage = filledFields / CountOfRequiredInfoFields * 100;
                return (int)fillPercentage;
            }
        }
    }

    /// <summary>
    /// Financial Support types for study residence permit
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLEOPI")]
    public enum OLEOPISupportTypes
    {
        /// <summary>
        /// Default value - unspecified
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Personal funding - rich family?
        /// </summary>
        PersonalFunds = 1,

        /// <summary>
        /// Grant or Scholarship funding
        /// </summary>
        ScholarshipGrant = 2,

        /// <summary>
        /// Benefit program provided by education institution
        /// </summary>
        EduInstitutionBenefits = 3,

        /// <summary>
        /// Job employment covers education (really?)
        /// </summary>
        JobEmployment = 4,

        /// <summary>
        /// Must specify manually - field is provided for that
        /// </summary>
        Other = 5
    }
}
