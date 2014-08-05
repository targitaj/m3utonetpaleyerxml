namespace Uma.Eservices.Models.OLE
{
    using System;

    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// Staying in Finland - dates, intentions
    /// </summary>

    public class OLEResidenceDurationBlock : BaseModel
    {
        /// <summary>
        /// Contains ID valuue of Application to be preserved between web calls
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Specifies whether user is already in Finland (reqiores additional attachment)
        /// </summary>
        public bool? AlreadyInFinland { get; set; }

        /// <summary>
        /// Date of arrival in Finland
        /// </summary>
        public DateTime? ArrivalDate { get; set; }

        /// <summary>
        /// Planned departure date
        /// </summary>
        public DateTime? DepartDate { get; set; }

        /// <summary>
        /// Free text of intentions about staying in finland - how long
        /// </summary>
        public string DurationOfStay { get; set; }
    }
}
