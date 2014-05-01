using System.ComponentModel.DataAnnotations;

namespace TILDE.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// The register view model.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Gets or sets the legal statuses.
        /// </summary>
        public IList<SelectListItem> LegalStatuses { get; set; }

        /// <summary>
        /// Gets or sets the legal status id.
        /// </summary>
        [Display(Name = "Juridiskais statuss")]
        public int LegalStatusId { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [Required]
        [Display(Name = "Vārds un uzvards, vai juridiskas personas nosaukums")]
        public string PersonName { get; set; }

        [Required]
        [Display(Name = "Personas kods, vai NMR kods, vai ārvalstīs registretas ID numurs")]
        public string PersonalCodeNmr { get; set; }

        [Required]
        [Display(Name = "LR rezidents")]
        public bool IsLRResident { get; set; }

        [Display(Name = "Ienakuma nodokla likme")]
        [Range(0, int.MaxValue, ErrorMessage = "Jabut pozitivam")]
        public int IncomeTaxRate { get; set; }

        [Display(Name = "Maksātnespējīga")]
        public bool IsInsolvent { get; set; }

        [Required]
        [Display(Name = "Adrese")]
        public string Address { get; set; }

        [Display(Name = "Elektroniskā apkārtraksta saņemšana")]
        public bool ReceiveNewsletter { get; set; }

        [Display(Name = "Nepilnīga informācija")]    
        public bool IsIncompleteInformation { get; set; }
    }
}
