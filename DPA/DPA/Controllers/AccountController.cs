using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DPA.Db;
using DPA.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using DPA.Models;

namespace DPA.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private AccountService _accountService = new AccountService();

        public AccountController()
        {
        }

        [AllowAnonymous]
        public ActionResult Search()
        {
            var model = new SearchViewModel();

            model.Persons = _accountService.SearchPerson("", "").Select(s => new RegisterViewModel() { PersonName = s.PersonName, PersonalCodeNmr = s.PersonalCodeNmr }).ToList();
            //model.PersonName = "asd";
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Search(SearchViewModel model)
        {
            model.Persons = _accountService.SearchPerson(model.PersonName, model.PersonalCodeNmr).Select(s => new RegisterViewModel() { PersonName = s.PersonName, PersonalCodeNmr = s.PersonalCodeNmr }).ToList();

            return Json(model);
        }

        [AllowAnonymous]
        public ActionResult Department()
        {
            var model = new DepartmentViewModel();

            PrepareDepartmentViewModel(model);

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Department(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                _accountService.AddDepartment(new Department()
                {
                    Address = model.Address,
                    ParentId = model.ParentDepartmentId,
                    Name = model.Name
                });
            }

            PrepareDepartmentViewModel(model);

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private void PrepareDepartmentViewModel(DepartmentViewModel model)
        {
            model.
                Departments =
                _accountService.Departments.Select(
                    s =>
                        new SelectListItem() { Text = s.Name, Value = s.Id.ToString(CultureInfo.InvariantCulture) })
                    .ToList();

            model.Departments.Insert(0, new SelectListItem() { Text = "-", Value = "" });
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterViewModel();

            PrepareRegisterViewModel(model);

            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                _accountService.AddPerson(new Person()
                {
                    Address = model.Address,
                    IncomeTaxRate = model.IncomeTaxRate,
                    IsIncompleteInformation = model.IsIncompleteInformation,
                    IsInsolvent = model.IsInsolvent,
                    IsLRResident = model.IsLRResident,
                    LegalStatusId = model.LegalStatusId,
                    PersonName = model.PersonName,
                    ReceiveNewsletter = model.ReceiveNewsletter,
                    PersonalCodeNmr = model.PersonalCodeNmr
                });
            }

            PrepareRegisterViewModel(model);
            
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private void PrepareRegisterViewModel(RegisterViewModel model)
        {
            model.
                LegalStatuses =
                _accountService.LegalStatuses.Select(
                    s =>
                        new SelectListItem() { Text = s.Description, Value = s.Id.ToString(CultureInfo.InvariantCulture) })
                    .ToList();
        }

    }
}