namespace Uma.Eservices.Models.OLE
{
    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// Wrapper model to make dynamic CurrentCiizenship control helper
    /// </summary>
    public class OLEPreviousCitizenship : BaseModel
    {
        /// <summary>
        /// Information about previus citizenship
        /// </summary>
        public string PreviousCitizenship { get; set; }

        /// <summary>
        /// Current citizenship LABEL value
        /// </summary>
        public int Id { get; set; }
    }
}
