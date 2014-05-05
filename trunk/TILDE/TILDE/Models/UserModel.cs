using System.ComponentModel.DataAnnotations;

namespace TILDE.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class UserModel
    {
        [Display(Name = "User Name")]
        public IList<SelectListItem> Users { get; set; }

        public int? UserId { get; set; }

        [Display(Name = "Is Balance Positive")]
        public bool IsBalancePositive { get; set; }

        [Display(Name = "Largest Borrower User Name")]
        public string LargestBorrowerUserName { get; set; }

        [Display(Name = "Largest Borrower Amount")]
        public int LargestBorrowerAmount { get; set; }

        [Display(Name = "Largest Creditor User Name")]
        public string LargestCreditorUserName { get; set; }

        [Display(Name = "Largest Creditor Amount")]
        public int LargestCreditorAmount { get; set; }

        [Display(Name = "Borrowing Average Amount")]
        public decimal BorrowingAverageAmount { get; set; }
    }
}
