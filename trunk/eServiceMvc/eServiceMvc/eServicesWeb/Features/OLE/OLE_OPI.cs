namespace Uma.Eservices.Web.Features.OLE
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;

    using Microsoft.Ajax.Utilities;
    using Microsoft.AspNet.Identity;

    using Uma.Eservices.Logic.Features;
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.Models.OLE;
    using Uma.Eservices.Models.OLE.Enums;
    using Uma.Eservices.Web.Components;
    using Uma.Eservices.Web.Components.PdfViewCreator;
    using Uma.Eservices.Web.Components.Vetuma;
    using Uma.Eservices.Web.Core.Filters;

    /// <summary>
    /// Controller for OLE pages
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "This is too complex class on itself. Dividing it will make things even more complex")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Required for Routing to work")]
    public partial class OLEController
    {
        /// <summary>
        /// Default entry point for Study Residence permit applications
        /// redirects to Step1 where looking for existing or creating new app is happening
        /// </summary>
        /// <returns>Step1 with either existing or new application model</returns>
        [HttpGet]
        public virtual ActionResult OPI()
        {
            return this.RedirectPermanent(Url.Action(MVC.OLE.OPIStep1(0)));
        }

        /// <summary>
        /// Step 1 of OLE OPI (Student Form) application
        /// </summary>
        /// <param name="id">
        /// The unique identifier of application in database.
        /// If 0 (zero) is passed - user gets routed to new application create and prefill with redirect to this back
        /// </param>
        [HttpGet]
        [Route("opi/{id}")]
        [Route("opi/{id}/Step1", Name = "ole_opi_step1")]
        [DatabaseTransaction]
        public virtual ActionResult OPIStep1(int id)
        {

            if (id == 0)
            {
                return this.LoadOrCreateAndPrefillNewApplication();
            }

            OLEPersonalInformationPage model = this.businessLogic.GetPersonalInformationPageModel(id);
            return this.View(MVC.OLE.Views.Views.OPI_Step1, model);
        }

        /// <summary>
        /// Receives the Fisrt page in OLE OPI application and saves it.
        /// Then does redirect to next page.
        /// </summary>
        /// <param name="model">The first page = person data model.</param>
        [HttpPost]
        [DatabaseTransaction]
        [ValidateAntiForgeryToken]
        public virtual ActionResult OPIStep1Save(OLEPersonalInformationPage model)
        {
            if (model == null || model.ApplicationId == 0)
            {
                return this.LoadOrCreateAndPrefillNewApplication();
            }

            this.businessLogic.SavePersonalInformationPageModel(model);

            if (!this.ModelState.IsValid)
            {
                var modelTemp = this.businessLogic.GetPersonalInformationPageModel(model.ApplicationId);
                return this.View(MVC.OLE.Views.Views.OPI_Step1, modelTemp);
            }
            
            return this.RedirectToRoutePermanent("ole_opi_step2", new { controller = "OLE", id = model.ApplicationId });
        }

        /// <summary>
        /// Using Ajax
        /// Receives the Fisrt page in OLE OPI application and saves it.
        /// Then does redirect to next page.
        /// </summary>
        /// <param name="model">The first page = person data model.</param>
        [HttpPost]
        [DatabaseTransaction]
        [ValidateAntiForgeryToken]
        public virtual ActionResult OPIStep1AjaxSave(OLEPersonalInformationPage model)
        {
            OPIStep1Save(model);

            return this.JsonPublic(new { result = true });
        }

        /// <summary>
        /// Calls for 2rd step of Study Residence permit page - Study information
        /// </summary>
        /// <param name="id">The identifier of application.</param>
        [HttpGet]
        [Route("opi/{id}/Step2", Name = "ole_opi_step2")]
        [DatabaseTransaction]
        public virtual ActionResult OPIStep2(int id)
        {
            if (id == 0)
            {
                return this.LoadOrCreateAndPrefillNewApplication();
            }

            OLEOPIEducationInformationPage model = this.businessLogic.GetEducationInformationPageModel(id);
            return this.View(MVC.OLE.Views.Views.OPI_Step2, model);
        }

        /// <summary>
        /// Receives the Second (education) page in OLE OPI application and saves it.
        /// Then does redirect to next page.
        /// </summary>
        /// <param name="model">The first page = education data model.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DatabaseTransaction]
        public virtual ActionResult OPIStep2Save(OLEOPIEducationInformationPage model)
        {
            if (model == null || model.ApplicationId == 0)
            {
                return this.LoadOrCreateAndPrefillNewApplication();
            }

            this.businessLogic.SaveEducationInformationPage(model);

            if (!this.ModelState.IsValid)
            {
                // OLEOPIEducationInformationPage modelTemp = this.businessLogic.GetEducationInformationPageModel(model.ApplicationId);
                // return this.View(MVC.OLE.Views.Views.OPI_Step2, modelTemp);
            }

            return this.RedirectToRoutePermanent("ole_opi_step3", new { controller = "OLE", id = model.ApplicationId });
        }

        /// <summary>
        /// Using Ajax
        /// Receives the Second (education) page in OLE OPI application and saves it.
        /// Then does redirect to next page.
        /// </summary>
        /// <param name="model">The first page = education data model.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DatabaseTransaction]
        public virtual ActionResult OPIStep2AjaxSave(OLEOPIEducationInformationPage model)
        {
            OPIStep2Save(model);

            return this.JsonPublic(new { result = true });
        }

        /// <summary>
        /// Calls for 3rd step of Study Residence permit page - Credit and Health insurance page
        /// </summary>
        /// <param name="id">The identifier of application.</param>
        [HttpGet]
        [Route("opi/{id}/Step3", Name = "ole_opi_step3")]
        [DatabaseTransaction]
        public virtual ActionResult OPIStep3(int id)
        {
            if (id == 0)
            {
                return this.LoadOrCreateAndPrefillNewApplication();
            }

            OLEOPIFinancialInformationPage model = this.businessLogic.GetFinancialsInformationPageModel(id);
            return this.View(MVC.OLE.Views.Views.OPI_Step3, model);
        }

        /// <summary>
        /// Receives the Third (finance/insurance and criminal) page in OLE OPI application and saves it.
        /// Then does redirect to next page.
        /// </summary>
        /// <param name="model">The third page = finance/insurance and criminal data model.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult OPIStep3Save(OLEOPIFinancialInformationPage model)
        {
            if (model == null || model.ApplicationId == 0)
            {
                return this.LoadOrCreateAndPrefillNewApplication();
            }

            this.businessLogic.SaveOLEOPIFinancialInformationPage(model);

            if (!this.ModelState.IsValid)
            {
                //OLEOPIFinancialInformationPage modelTemp = this.businessLogic.GetFinancialsInformationPageModel(model.ApplicationId);
                //return this.View(MVC.OLE.Views.Views.OPI_Step3, modelTemp);
            }

            return this.RedirectToRoutePermanent("ole_opi_step4", new { controller = "OLE", id = model.ApplicationId });
        }

        /// <summary>
        /// Using Ajax
        /// Receives the Third (finance/insurance and criminal) page in OLE OPI application and saves it.
        /// Then does redirect to next page.
        /// </summary>
        /// <param name="model">The third page = finance/insurance and criminal data model.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DatabaseTransaction]
        public virtual ActionResult OPIStep3AjaxSave(OLEOPIFinancialInformationPage model)
        {
            OPIStep3Save(model);

            return this.JsonPublic(new { result = true });
        }

        /// <summary>
        /// Step 1 of OLE OPI (Student Form) application
        /// </summary>
        /// <param name="id">
        /// The unique identifier of application in database.
        /// If 0 (zero) is passed - user gets routed to new application create and prefill with redirect to this back
        /// </param>
        [HttpGet]
        [Route("opi/{id}/Step4", Name = "ole_opi_step4")]
        public virtual ActionResult OPIStep4(int id)
        {
            if (id == 0)
            {
                this.LoadOrCreateAndPrefillNewApplication();
            }

            OLEAttachmentPage model = this.businessLogic.GetAttachmentPageModel(id);
            return this.View(MVC.OLE.Views.Views.OPI_Step4, model);
        }

        /// <summary>
        /// Receives the Forth page in OLE OPI application and saves it.
        /// Then does redirect to next page.
        /// </summary>
        /// <param name="model">The first page = person data model.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult OPIStep4Save(OLEAttachmentPage model)
        {
            if (model == null || model.ApplicationId == 0)
            {
                return this.LoadOrCreateAndPrefillNewApplication();
            }

            // TODO: Validate & save passed model into database

            return this.RedirectToRoutePermanent("ole_opi_step5", new { controller = "OLE", id = model.ApplicationId });
        }

        /// <summary>
        /// Using Ajax
        /// Receives the Forth page in OLE OPI application and saves it.
        /// Then does redirect to next page.
        /// </summary>
        /// <param name="model">The first page = person data model.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DatabaseTransaction]
        public virtual ActionResult OPIStep4AjaxSave(OLEAttachmentPage model)
        {
            OPIStep4Save(model);

            return this.JsonPublic(new { result = true });
        }


        /// <summary>
        /// Step 5 of OLE OPI (Student Form) application
        /// </summary>
        /// <param name="id">
        /// The unique identifier of application in database.
        /// If 0 (zero) is passed - user gets routed to new application create and prefill with redirect to this back
        /// </param>
        [HttpGet]
        [Route("opi/{id}/Step5", Name = "ole_opi_step5")]
        public virtual ActionResult OPIStep5(int id)
        {
            if (id == 0)
            {
                this.LoadOrCreateAndPrefillNewApplication();
            }

            OLEOPIValidationPage model = this.businessLogic.GetValidationPageModel(id);
            return this.View(MVC.OLE.Views.Views.OPI_Step5, model);
        }

        /// <summary>
        /// Receives the Fifth (validation) page in OLE OPI application and saves it.
        /// Then does redirect to next page.
        /// </summary>
        /// <param name="model">The fifth page = validation page model.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult OPIStep5Save(OLEAttachmentPage model)
        {
            if (model == null || model.ApplicationId == 0)
            {
                return this.LoadOrCreateAndPrefillNewApplication();
            }

            if (this.PaymentLogic.IsApplicationPaid(model.ApplicationId))
            {
                return this.RedirectToActionPermanent(MVC.Dashboard.ActionNames.Index2, MVC.Dashboard.Name);
            }

            return this.RedirectToRoutePermanent("ole_opi_step6", new { controller = "OLE", id = model.ApplicationId });
        }

        /// <summary>
        /// Step 6 of OLE OPI (Student Form) application, Payment page
        /// </summary>
        /// <param name="id">
        /// The unique identifier of application in database.
        /// If 0 (zero) is passed - user gets routed to new application create and prefill with redirect to this back
        /// </param>
        [HttpGet]
        [Route("opi/{id}/Step6", Name = "ole_opi_step6")]
        public virtual ActionResult OPIStep6(int id)
        {
            if (id == 0)
            {
                return this.LoadOrCreateAndPrefillNewApplication();
            }

            var model = this.businessLogic.GetPaymentModel(id);
            model.ApplicationId = id;
            return this.View(MVC.OLE.Views.Views.OPI_Step6, model);
        }

        /// <summary>
        /// Receives the sixth (payment) page.
        /// Then does redirect to Vetuma page.
        /// </summary>
        /// <param name="model">The sixth page = payment data model.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual EmptyResult OPIStep6Save(OLEPaymentPage model)
        {
            if (model == null || model.ApplicationId == 0)
            {
                throw new ArgumentException("applicationId is 0 or model is null");
            }

            var paymentsHelper = new VetumaPaymentHelper(this.PaymentLogic);

            paymentsHelper.SubmitPayment(model.ApplicationId, model.SelectedEmbassy);
            return new EmptyResult();
        }

        /// <summary>
        /// Step 7 of OLE OPI (Student Form) application, submit page
        /// </summary>
        /// <param name="id">
        /// The unique identifier of application in database.
        /// If 0 (zero) is passed - user gets routed to new application create and prefill with redirect to this back
        /// </param>
        [HttpPost]
        [Route("opi/{id}/Step7", Name = "ole_opi_step7")]
        public virtual ActionResult OPIStep7(int id)
        {
            if (id == 0)
            {
                return this.LoadOrCreateAndPrefillNewApplication();
            }

            // UI check uncomment to skip payment process
            // OLEApplicationSubmit model = this.businessLogic.GetOLEApplicationSubmit(id);
            // return this.View(MVC.OLE.Views.Views.OPI_Step7, model);

            var paymentHelper = new VetumaPaymentHelper(this.PaymentLogic);

            if (paymentHelper.ProcessResult(id))
            {
                OLEApplicationSubmit model = this.businessLogic.GetOLEApplicationSubmit(id);
                return this.View(MVC.OLE.Views.Views.OPI_Step7, model);
            }

            return this.RedirectToAction(MVC.Dashboard.ActionNames.Index);
        }

        /// <summary>
        /// Receives the seventh (submit) page.
        /// </summary>
        /// <param name="model">The seventh page = submit data model.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult OPIStep7Save(OLEApplicationSubmit model)
        {
            if (model == null || model.ApplicationId == 0)
            {
                throw new ArgumentException("applicationId is 0 or model is null");
            }
            this.PdfCtr.CreatePdfAsync(model.ApplicationId, this.ControllerContext, this.TempData, "PdfViews/PDF_OLE_OPI");

            this.formsCommonLogic.SubmitForm(model.ApplicationId);
            return this.RedirectToActionPermanent(MVC.Dashboard.ActionNames.Index2, MVC.Dashboard.Name);
        }

        /// <summary>
        /// Checks whether user has already existing OLE/OPI draft and loads it (if is any)
        /// Creates new application if none draft exists, prefills it (if needed) and reloads its first page
        /// </summary>
        /// <returns>First page of OLE/OPI with either existing draft or new application draft</returns>
        private RedirectToRouteResult LoadOrCreateAndPrefillNewApplication()
        {
            // TODO: Check DB whether user already has draft and load it if found
            int? existingApplicationId = this.formsCommonLogic.GetExistingApplicationId(FormType.OPIStudyResidencePermit);
            if (existingApplicationId.HasValue)
            {
                return this.RedirectToRoutePermanent("ole_opi_step1", new { controller = "OLE", id = existingApplicationId.Value });
            }

            // TODO: Prefill new application draft, save it in DB for current user and reload application form with its ID
            int createdApplicationId = this.formsCommonLogic.CreateNewApplication(FormType.OPIStudyResidencePermit, int.Parse(User.Identity.GetUserId()));

            // Redirecting with URL rewrite to reload prefilled new form
            return this.RedirectToRoutePermanent("ole_opi_step1", new { controller = "OLE", id = createdApplicationId });
        }

        /// <summary>
        /// Uploadin file to server and saving it to file system and DB
        /// </summary>
        /// <param name="attachmentType">Type of attachment block</param>
        /// <param name="applicationId">Main application Id</param>
        /// <param name="model">Attachment block bodel</param>
        [DatabaseTransaction]
        public virtual ActionResult UploadFile(AttachmentTypeEnum attachmentType, int applicationId, AttachmentBlock model)
        {
            return FileSaver.UploadFile(
                this,
                MVC.OLE.ActionNames.DeleteFile,
                MVC.OLE.ActionNames.DownloadFile,
                attachmentType,
                businessLogic,
                applicationId,
                model);
        }

        /// <summary>
        /// Used for file deletion
        /// </summary>
        /// <param name="fileUrl">
        /// The url to the file.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "need string"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "fileUrl", Justification = "need string"), HttpPost]
        [DatabaseTransaction]
        public virtual ActionResult DeleteFile(string fileUrl)
        {
            return FileSaver.DeleteFile(this, fileUrl, businessLogic);
        }

        /// <summary>
        /// The download file function from file upload screen can be used some where else.
        /// </summary>
        /// <param name="fileUrl">
        /// The file url for downloading.
        /// </param>
        /// <param name="mimetype">
        /// The mimetype.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "need string"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "fileUrl", Justification = "need string")]
        public virtual ActionResult DownloadFile(string fileUrl, string mimetype)
        {
            return FileSaver.DownloadFile(this, fileUrl, mimetype);
        }
    }
}