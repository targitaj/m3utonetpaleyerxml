namespace Uma.Eservices.Models.OLE
{
    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// Wrapper model to make dynamic CurrentCiizenship control helper
    /// </summary>
    public class OLECurrentCitizenship : BaseModel
    {
        /// <summary>
        /// Current citizenship LABEL value
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Information about current citizenship
        /// </summary>
        public string CurrentCitizenship { get; set; }
    }
}
