using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TILDE.Db;
using TILDE.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using TILDE.Models;
using System.Data.Linq;

namespace TILDE.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private AccountService _accountService = new AccountService();
        
        [AllowAnonymous]
        public ActionResult User()
        {
            return View(PrepareModel(new UserModel()));
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult User(UserModel model)
        {
            return View(PrepareModel(model));
        }

        private UserModel PrepareModel(UserModel model)
        {
            var users = _accountService.Users;

            model.Users =
                users.Select(s => new SelectListItem() { Text = s.UserName, Value = s.Id.ToString() }).ToList();

            if (!model.UserId.HasValue && model.Users.Count != 0)
            {
                model.UserId = int.Parse(model.Users.First().Value);
            }

            User user = null;

            if (model.UserId.HasValue)
            {
                var largestBorrower = _accountService.GetLargestBorrowerByUserId(model.UserId.Value);

                if (largestBorrower != null)
                {
                    model.LargestBorrowerAmount = largestBorrower.Total;
                    model.LargestBorrowerUserName = largestBorrower.User.UserName;
                }

                var largestCreditor = _accountService.GetLargestCreditorByUserId(model.UserId.Value);

                if (largestCreditor != null)
                {
                    model.LargestCreditorAmount = largestCreditor.Total;
                    model.LargestCreditorUserName = largestCreditor.User.UserName;
                }

                model.IsBalancePositive = model.LargestCreditorAmount <= model.LargestBorrowerAmount;
                model.BorrowingAverageAmount = _accountService.GetAvarageBorrowingByUserId(model.UserId.Value);
            }
            
            return model;
        }
    }
}