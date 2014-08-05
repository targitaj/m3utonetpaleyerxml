namespace Uma.Eservices.DbObjects.FormCommons
{
    /// <summary>
    /// Enumeration for person Gender specification
    /// </summary>
    public enum Gender : byte
    {
        /// <summary>
        /// Default value = not specified or unknown
        /// </summary>
        NotSpecified = 0,

        /// <summary>
        /// Gender = Male / Masculine
        /// </summary>
        Male = 1,

        /// <summary>
        /// Gender = Female / Feminine
        /// </summary>
        Female = 2
    }
}
