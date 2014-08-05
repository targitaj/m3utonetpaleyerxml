namespace Uma.Eservices.DbObjects.FormCommons
{
    /// <summary>
    /// Enumerator to determine language of communication with applyer
    /// One of three supported languages, default = EN
    /// </summary>
    public enum CommunicationLanguage : byte
    {
        /// <summary>
        /// English (EN) - Default 
        /// </summary>
        English = 0,

        /// <summary>
        /// Finnish (fi)
        /// </summary>
        Finnish = 1,

        /// <summary>
        /// Swedish (sv)
        /// </summary>
        Swedish = 2
    }
}
