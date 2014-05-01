using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DPA.Models
{
    public class DepartmentViewModel
    {
        public IList<SelectListItem> Departments { get; set; }

        [Required]
        [Display(Name = "Nosaukums")]
        public string Name { get; set; }

        [Display(Name = "Augstāka līmeņa struktūrvienība")]
        public int? ParentDepartmentId { get; set; }

        [Required]
        [Display(Name = "Adrese")]
        public string Address { get; set; }
    }
}