using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TILDE.Models
{
    public class AddressViewModel
    {
        public IList<SelectListItem> Regions { get; set; }
        public IList<SelectListItem> Parishes { get; set; }
        public IList<SelectListItem> Cities { get; set; }

        [Required]
        [Display(Name = "Novads")]
        public string RegionId { get; set; }

        [Display(Name = "Pagasts")]
        public int? ParishId { get; set; }

        [Required]
        [Display(Name = "Adrese")]
        public string Address { get; set; }
    }
}