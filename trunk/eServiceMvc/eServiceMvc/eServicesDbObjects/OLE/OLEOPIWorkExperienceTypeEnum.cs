namespace Uma.Eservices.DbObjects.OLE
{
    /// <summary>
    /// Work experience types in study field
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLEOPI")]
    public enum OLEOPIWorkExperienceType : byte
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
