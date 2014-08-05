namespace Uma.Eservices.Web.Features.Sandbox
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using Uma.Eservices.Common;
    using Uma.Eservices.Logic.Features;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Sandbox;
    using Uma.Eservices.Web.Components;
    using Uma.Eservices.Web.Components.PdfViewCreator;
    using Uma.Eservices.Web.Core;
    using Uma.Eservices.Web.Core.Filters;

    /// <summary>
    /// MVC Controller for Payground and test/Sample pages to e-Service application
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "I admit there are, its a test")]
    [ExcludeFromCodeCoverage]
    [AllowAnonymous]
    public partial class SandboxController : BaseController, IBaseController
    {
        /// <summary>
        /// The business logic reference injected through constructor via Unity IoC DI
        /// </summary>
        private readonly ISandboxLogic businessLogic;

        /// <summary>
        /// Gets or sets the logger component of Controller(s).
        /// </summary>
        [Dependency]
        public ILog Logger { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SandboxController" /> class.
        /// </summary>
        /// <param name="logic">The busines logic interface, injected through Unity with real or testable mock.</param>
        /// <param name="localizationManager">The localization manager - DB connector through injection.</param>
        public SandboxController(ISandboxLogic logic, ILocalizationManager localizationManager)
        {
            this.businessLogic = logic;
        }

        #region [Test form HTTP GET methods]

        /// <summary>
        /// Main entry point/view for Sandbox
        /// </summary>
        [HttpGet]
        public virtual ActionResult Index()
        {
            this.StoreReturnRoute();

            return this.View(MVC.Sandbox.Views.Index);
        }

        /// <summary>
        /// Page for testing various exceptions and error situations
        /// </summary>
        [HttpGet]
        public virtual ActionResult ExceptionTests()
        {
            return this.View(MVC.Sandbox.Views.Exceptions, new TestFormModel { FirstField = "First field test", ValidationField = "123 This is validated", RequiredField = "Required field test" });
        }

        /// <summary>
        /// Page for testing grid and typography
        /// </summary>
        [HttpGet]
        public virtual ActionResult Grid()
        {
            return this.View(MVC.Sandbox.Views.Grid);
        }

        /// <summary>
        /// POST action which raises native application Exception.
        /// To test how it logs parameters and stack with exception to Logging media(s)
        /// </summary>
        /// <param name="model">The model brought from View.</param>
        /// <exception cref="EserviceException">This is Dummy App exception throwed from Sandbox</exception>
        [HttpPost]
        [FormValidator]
        public virtual ActionResult ExceptionTestApp(TestFormModel model)
        {
            this.Logger.Info("There should be another Log record about purposely throwed Application exception");
            throw new EserviceException("This is Dummy App exception throwed from Sandbox");
        }

        /// <summary>
        /// GET action, that raises native .Net exception.
        /// </summary>
        /// <param name="number">The number - dummy parameter to test logging.</param>
        /// <param name="name">The name - dummy parameter to test logging.</param>
        /// <exception cref="System.NullReferenceException">Null reference from somewhere inside code (Sanbox Test)</exception>
        [HttpGet]
        [DebuggerStepThrough]
        public virtual ActionResult ExceptionTestNet(int number, string name)
        {
            this.Logger.Info("There should be another Log record about purposely throwed NullReference exception");
            throw new NullReferenceException("Null reference from somewhere inside code (Sanbox Test)");
        }

        /// <summary>
        /// Should throw [native?] Security exception
        /// </summary>
        /// <exception cref="EserviceException">This is Dummy Security exception throwed from Sandbox</exception>
        [HttpGet]
        public virtual ActionResult ExceptionTestSec()
        {
            this.Logger.Info("There should be another Log record about purposely throwed Security exception");
            throw new EserviceException("This is Dummy Security exception throwed from Sandbox");
        }

        /// <summary>
        /// Should call dummy view which has failing model - model binding exception tests (outside of controller)
        /// </summary>
        /// <exception cref="EserviceException">This is Dummy App exception throwed from Sandbox</exception>
        [HttpGet]
        public virtual ActionResult ExceptionTestView()
        {
            throw new EserviceException("This is Dummy App exception throwed from Sandbox");
        }

        /// <summary>
        /// Should call dummy view which has failing model - model binding exception tests (outside of controller)
        /// </summary>
        /// <param name="giveCount">Dummy mandatory (not nullable) parameter, which should not be supplied!.</param>
        [HttpGet]
        public virtual ActionResult ExceptionTestParameter(int giveCount)
        {
            // Should not get here, as exception on entry occurs
            return this.View(MVC.Sandbox.Views.Exceptions, new TestFormModel { FirstField = "First field test", ValidationField = "123 This is validated", RequiredField = "Required field test" });
        }

        /// <summary>
        /// Form to test translations and locale based field behavior - Date, Time, Number etc. Display/Entry
        /// </summary>
        [HttpGet]
        public virtual ActionResult LocalizationTests()
        {
            return this.View(MVC.Sandbox.Views.Localization);
        }

        /// <summary>
        /// Creation tests for PFD generation.
        /// </summary>
        /// <param name="id">The identifier of something.</param>
        [HttpGet]
        public virtual ActionResult PdfCreationTests(int? id)
        {
            var model = this.businessLogic.GetPdfSampleModel(id);
            return this.View(MVC.Sandbox.Views.PdfCreation, model);
        }

        /// <summary>
        /// Page to test File Upload functionalities
        /// </summary>
        [HttpGet]
        public virtual ActionResult FileUploadTests()
        {
            return this.View(MVC.Sandbox.Views.FileUpload, null);
        }

        /// <summary>
        /// Page to test File Upload One File functionalities
        /// </summary>
        [HttpGet]
        public virtual ActionResult FileUploadOneFileTests()
        {
            return this.View(MVC.Sandbox.Views.FileUploadOneFile, null);
        }

        /// <summary>
        /// Function to test file uploading ajax control
        /// </summary>
        /// <param name="entityId">
        /// The entity id is used for more then one controls.
        /// </param>
        public virtual ActionResult UploadFile(int? entityId)
        {
            // here we can send in some extra info to be included with the delete url 
            var statuses = new List<ViewDataUploadFileResult>();
            for (var i = 0; i < Request.Files.Count; i++)
            {
                var st = FileSaver.StoreFile(new FileSaveModel()
                {
                    File = Request.Files[i],
                    // note how we are adding an additional value to be posted with delete request

                    // and giving it the same value posted with upload
                    DeleteUrl = Url.Action("DeleteFile", new { entityId = entityId }),
                    StorageDirectory = Server.MapPath("~/Content/uploads"),
                    UrlPrefix = ResolveServerUrl(VirtualPathUtility.ToAbsolute("~/Content/uploads"), false), // this is used to generate the relative url of the file

                    // overriding defaults
                    FileName = Request.Files[i].FileName, // default is filename suffixed with filetimestamp
                    ThrowExceptions = true // default is false, if false exception message is set in error property
                });

                statuses.Add(st);
            }

            // statuses contains all the uploaded files details (if error occurs then check error property is not null or empty)
            // todo: add additional code to generate thumbnail for videos, associate files with entities etc

            // adding thumbnail url for jquery file upload javascript plugin
            statuses.ForEach(x => x.ThumbnailUrl = x.Url + string.Empty); // uses ImageResizer httpmodule to resize images from this url

            // setting custom download url instead of direct url to file which is default
            statuses.ForEach(x => x.Url = Url.Action("DownloadFile", new { fileUrl = x.Url, mimetype = x.FileType }));

            var viewresult = Json(new { files = statuses });
            // for IE8 which does not accept application/json
            if (Request.Headers["Accept"] != null && !Request.Headers["Accept"].Contains("application/json"))
            {
                viewresult.ContentType = "text/plain";
            }

            return viewresult;
        }

        /// <summary>
        /// Used for file deletion
        /// </summary>
        /// <param name="fileUrl">
        /// The url to the file.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "need string"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "fileUrl", Justification = "need string"), HttpPost]
        public virtual ActionResult DeleteFile(string fileUrl)
        {
            var uri = new Uri(fileUrl);
            var filePath = Server.MapPath(uri.LocalPath);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            var viewresult = Json(new { error = string.Empty });
            // for IE8 which does not accept application/json
            if (Request.Headers["Accept"] != null && !Request.Headers["Accept"].Contains("application/json"))
            {
                viewresult.ContentType = "text/plain";
            }

            return viewresult; // trigger success
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
            var uri = new Uri(fileUrl);
            var filePath = Server.MapPath(uri.LocalPath);

            if (System.IO.File.Exists(filePath))
            {
                return this.File(filePath, mimetype);
            }

            return new HttpNotFoundResult("File not found");
        }

        /// <summary>
        /// Sample and playground to produce standard 1-column form.
        /// </summary>
        /// <param name="id">The fake identifier for something to load.</param>
        [HttpGet]
        [DatabaseTransaction]
        public virtual ActionResult TestForm(int? id)
        {
            var model = this.businessLogic.GetTestFormModel(id);
            return this.View(MVC.Sandbox.Views.TestForm, model);
        }

        /// <summary>
        /// Sample to test from with collapsing
        /// </summary>
        [HttpGet]
        public virtual ActionResult CollapseTest()
        {
            return this.View(MVC.Sandbox.Views.Collapse);
        }

        #endregion

        #region [HTTP POST handlers from Test pages]

        /// <summary>
        /// Receives the Single Column Form results on server side.
        /// </summary>
        /// <param name="model">The model of the form - its properties must be filled with values from Form fields.</param>
        [HttpPost]
        public virtual ActionResult TestFormSave(TestFormModel model)
        {
            // classical ModelState.IsValid should be handled by [FormValidator] attribute already
            // calling dummy BL method to mimic data persistence to somewhere
            this.businessLogic.SaveTestForm(model);

            // Fulfilling PRG pattern - here we passing fake ID to initiate GET View hydratation
            return this.RedirectToAction(MVC.Sandbox.TestForm());
        }

        /// <summary>
        /// Sample to test from with collapsing
        /// </summary>
        /// <param name="model">Model that used to test collapse feature</param>
        [HttpPost]
        [FormValidator]
        public virtual ActionResult CollapseTest(CollapseModel model)
        {
            return this.RedirectToAction(MVC.Sandbox.CollapseTest());
        }

        #endregion

        /// <summary>
        /// Pdf Creation test action. This action is used to create pdf document from view.
        /// Action will create two pdf files. One will be generated from class via HtmlHelper 
        /// Other from csHtml file.
        /// </summary>
        /// <param name="model">Kan7Model for post</param>
        /// <returns>View...  redirect</returns>
        [HttpPost]
        [FormValidator]
        public virtual ActionResult PdfCreationTests(Kan7Model model)
        {
            if (model == null)
            {
                return this.RedirectToAction(MVC.Sandbox.PdfCreationTests(this.businessLogic.SavePdfSampleModel(new Kan7Model())));
            }

            // PdfView pview = new PdfView();
            // pview.SetValue(model, "PdfReports/Kan7PdfReport", this.ControllerContext);
            // var res = pview.GetHtmlViewAsync();

            // Generate from HTML raxor fiew
            // PdfGenerator pdf = new PdfGenerator(res.Result);
            // pdf.PdfFileLocationPath = @"C:\pdf";
            // pdf.GeneratePdf();

            // // Generate pdf from Model
            // PdfGenerator pdf2 = new PdfGenerator(model.ToHtmlString());
            // pdf2.PdfFileLocationPath = @"C:\pdf";
            // pdf2.GeneratePdf();

            this.WebMessages.AddInfoMessage("Back From PDf", "__________________");
            return this.RedirectToAction(MVC.Sandbox.PdfCreationTests(this.businessLogic.SavePdfSampleModel(model)));
        }

        /// <summary>
        /// Page to test File Upload One File functionalities
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        [HttpPost]
        public virtual ActionResult FileUploadOneFileTests(FileUploadModel model)
        {
            return this.RedirectToAction(MVC.Sandbox.FileUploadOneFileTests());
        }

        /// <summary>
        /// Action to handle File Upload through Model
        /// </summary>
        /// <param name="model">The model of file upload.</param>
        [HttpPost]
        public virtual ActionResult FileUploadTests(FileUploadModel model)
        {
            if (model == null)
            {
                return this.View(MVC.Sandbox.Views.FileUpload, new FileUploadModel());
            }

            return this.View(MVC.Sandbox.Views.FileUploadOneFile, model);
        }

        /// <summary>
        /// The resolve full url specefied short url.
        /// </summary>
        /// <param name="absolutetUrl">
        /// The application absolute url.
        /// </param>
        /// <param name="forceHttps">
        /// The force use https.
        /// </param>
        private static Uri ResolveServerUrl(string absolutetUrl, bool forceHttps)
        {
            if (absolutetUrl.IndexOf("://", StringComparison.Ordinal) > -1)
            {
                return new Uri(absolutetUrl);
            }

            string newUrl = absolutetUrl;
            Uri originalUri = System.Web.HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) +
                "://" + originalUri.Authority + newUrl;
            return new Uri(newUrl);
        }
    }
}