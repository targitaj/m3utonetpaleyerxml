namespace Uma.Eservices.DbObjects.OLE
{
    /// <summary>
    /// Financial Support types for study residence permit
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLEOPI")]
    public enum OLEOPISupportTypes : byte
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
