namespace Uma.Eservices.Web.Features.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;

    using Uma.Eservices.Logic.Features.Dashboard;
    using Uma.Eservices.Web.Components;
    using Uma.Eservices.Web.Core;
    using System.IO;

    /// <summary>
    /// User dashboard controller
    /// </summary>
    public partial class DashboardController : BaseController
    {
        /// <summary>
        /// The business logic reference injected through constructor via Unity IoC DI
        /// </summary>
        private readonly IDashboardLogic businessLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController" /> class.
        /// </summary>
        /// <param name="logic">The busines logic interface, injected through Unity with real or testable mock.</param>
        public DashboardController(IDashboardLogic logic)
        {
            this.businessLogic = logic;
        }

        /// <summary>
        /// Main entry point for Dashboard
        /// </summary>
        public virtual ActionResult Index()
        {
            return this.View(MVC.Dashboard.Views.Dashboard, true);
        }

        /// <summary>
        /// Entry point for Dashboard from form submit -> fake entry
        /// </summary>
        public virtual ActionResult Index2()
        {
            return this.View(MVC.Dashboard.Views.Dashboard, businessLogic.LoadDocuments(int.Parse(User.Identity.GetUserId())));
        }

        /// <summary>
        /// New Application entry point for Dashboard
        /// </summary>
        public virtual ActionResult NewApplication()
        {
            return this.View(MVC.Common.Views._ApplicationTypes, true);
        }

        /// <summary>
        /// Residence permit entry point for Dashboard
        /// </summary>
        public virtual ActionResult ResidencePermit()
        {
            return this.View(MVC.Info.Views.ResidencePermitTypeSelection, true);
        }

        /// <summary>
        /// Apply for work entry point for Dashboard
        /// </summary>
        public virtual ActionResult ResidenceForWork()
        {
            return this.View(MVC.Info.Views.ResidencePermitForWork, true);
        }

        /// <summary>
        /// Apply for Specialist entry point for Dashboard
        /// </summary>
        public virtual ActionResult ResidenceSpecialist()
        {
            return this.View(MVC.Info.Views.ResidencePermitSpecialist, true);
        }

        /// <summary>
        /// Apply for Study entry point for Dashboard
        /// </summary>
        public virtual ActionResult ResidenceStudy()
        {
            return this.View(MVC.Info.Views.ResidencePermitStudy, true);
        }

        /// <summary>
        /// Entry point for Supplemental Documents
        /// </summary>
        public virtual ActionResult SupplementalDocuments()
        {
            return this.View(
                MVC.Dashboard.Views.SupplementalDocuments,
                this.businessLogic.SupplementalDocumentModel);
        }

        /// <summary>
        /// Method to upload supplemental file for user application
        /// </summary>
        /// <param name="entityId">ID of a file added</param>
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
            // var uri = new Uri(fileUrl);
            var filePath = Server.MapPath(fileUrl);

            if (System.IO.File.Exists(filePath))
            {
                return this.File(filePath, mimetype);
            }

            return new HttpNotFoundResult("File not found");
        }
    }
}