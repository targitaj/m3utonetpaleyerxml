namespace Uma.Eservices.Models.OLE
{
    /// <summary>
    /// Block to describe any previous studies and work experience in field
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLEOPI")]
    public class OLEOPIPreviousStudiesBlock
    {
        /// <summary>
        /// Description field to specify any previous studies
        /// </summary>
        public string PreviousStudies { get; set; }

        /// <summary>
        /// Description of how previous studies are conected to ones applicant is applying to
        /// </summary>
        public string PreviousStudiesConnectionToCurrent { get; set; }

        /// <summary>
        /// Status value of Enum what work experience applicant has in regard to previous (only?) studies
        /// </summary>
        public OLEOPIWorkExperienceType WorkExperienceStatus { get; set; }

        /// <summary>
        /// Some choices of <see cref="WorkExperienceStatus"/> requires that they should be described
        /// </summary>
        public string WorkExperienceDescription { get; set; }

        /// <summary>
        /// Contains logic to determine on what percentage this data block is filled. 
        /// Should return number from 0 to 100 (%)
        /// </summary>
        public int BlockFillPercentage
        {
            get
            {
                // This may be replaced with validator-related logic
                return 100;
            }
        }
    }

    /// <summary>
    /// Work experience types in study field
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLEOPI")]
    public enum OLEOPIWorkExperienceType
    {
        /// <summary>
        /// Default should not be used/selected
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Have experience, should be described
        /// </summary>
        HaveExperience = 1,

        /// <summary>
        /// Does not have experience
        /// </summary>
        NotHaveexperience = 2,

        /// <summary>
        /// Some other work experience (not related to field) shoule be described
        /// </summary>
        OtherWorkExperience = 3
    }
}
