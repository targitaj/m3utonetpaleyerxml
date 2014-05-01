using System.ComponentModel.DataAnnotations;

namespace DPA.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// The register view model.
    /// </summary>
    public class SearchViewModel
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [Display(Name = "Vārds un uzvards, vai juridiskas personas nosaukums")]
        public string PersonName { get; set; }

        [Display(Name = "Personas kods, vai NMR kods, vai ārvalstīs registretas ID numurs")]
        public string PersonalCodeNmr { get; set; }

        public IList<RegisterViewModel> Persons { get; set; }
    }
}
