namespace Uma.Eservices.Web.Features.Info
{
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using Uma.Eservices.Logic.Features.HelpSupport;
    using Uma.Eservices.Models.Account;
    using Uma.Eservices.Models.Localization;
    using Uma.Eservices.Web.Core;
    using Uma.Eservices.Web.Core.Filters;

    /// <summary>
    /// Simple Informational pages controller
    /// </summary>
    public partial class InfoController : BaseController
    {
        /// <summary>
        /// Gets or sets the help logic (from Logic project).
        /// </summary>
        [Dependency]
        public IHelpSupportLogic HelpLogic { get; set; }

        /// <summary>
        /// Default entry point for controller.
        /// This should redirect back to homa page as all info HUB is located there
        /// </summary>
        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            return this.RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
        }

        /// <summary>
        /// Opens a informational  Help page.
        /// Page contains FAQ and answers
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public virtual ViewResult Help()
        {
            var res = this.HelpLogic.GetAllQuestionsAllAnswers();
            return this.View(MVC.Info.Views.Help, res);
        }

        /// <summary>
        /// Opens a informational page about Terms of Service
        /// </summary>
        [AllowAnonymous]
        public virtual ViewResult Terms()
        {
            return this.View(MVC.Info.Views.Terms);
        }

        /// <summary>
        /// Opens a informational page about Privacy
        /// </summary>
        [AllowAnonymous]
        public virtual ViewResult Privacy()
        {
            return this.View(MVC.Info.Views.Privacy);
        }

        /// <summary>
        /// Opens a informational page about Residence Permit types
        /// for user to choose either Work or Study permit
        /// </summary>
        [AllowAnonymous]
        public virtual ViewResult ResidencePermit()
        {
            return this.View(MVC.Info.Views.ResidencePermitTypeSelection, false);
        }

        /// <summary>
        /// Informational page of Work Residence permits
        /// </summary>
        [AllowAnonymous]
        public virtual ViewResult WorkPermit()
        {
            return this.View(MVC.Info.Views.ResidencePermitForWork, false);
        }

        /// <summary>
        /// Information page for Study Permit types and getting ones
        /// </summary>
        [AllowAnonymous]
        public virtual ViewResult StudyPermit()
        {
            return this.View(MVC.Info.Views.ResidencePermitStudy, false);
        }

        /// <summary>
        /// Informational page of Work Residence permits for Speicalists
        /// </summary>
        [AllowAnonymous]
        public virtual ViewResult WorkPermitSpecialist()
        {
            return this.View(MVC.Info.Views.ResidencePermitSpecialist, false);
        }
    }
}