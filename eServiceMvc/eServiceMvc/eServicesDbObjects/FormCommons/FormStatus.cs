namespace Uma.Eservices.DbObjects.FormCommons
{
    /// <summary>
    /// Enumeration that describes form status.
    /// </summary>
    public enum FormStatus : short
    {
        /// <summary>
        /// Draft - Eser opens new form but do not pay for it
        /// </summary>
        Draft = 0,

        /// <summary>
        /// User have paid for application
        /// </summary>
        Submited = 1,

        /// <summary>
        /// User have paid -> and form is sent to uma
        /// </summary>
        SentToUma = 2
    }
}
