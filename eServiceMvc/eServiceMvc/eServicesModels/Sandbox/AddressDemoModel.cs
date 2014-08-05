namespace Uma.Eservices.Models.Sandbox
{
    using System.Collections.Generic;

    /// <summary>
    /// Demo model for addresses
    /// </summary>
    public class AddressDemoModel
    {
        /// <summary>
        /// Preloaded list of Countries (from UMA)
        /// </summary>
        public Dictionary<string, string> CountryList { get; set; }

        /// <summary>
        /// Holds selected value of a Country from Dropdown
        /// </summary>
        public string CountrySelection { get; set; }
    }
}
