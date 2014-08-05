namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Uma.Eservices.Logic.Features.Common;
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.Models.OLE.Enums;
    using Uma.Eservices.Web.Core;

    /// <summary>
    /// The file saver.
    /// </summary>
    public static class FileSaver
    {
        /// <summary>
        /// Saving files to drive and return result off file saving
        /// </summary>
        /// <param name="fileSaveModel">
        /// Model that contains data about file.
        /// </param>
        public static ViewDataUploadFileResult StoreFile(FileSaveModel fileSaveModel)
        {
            if (fileSaveModel == null)
            {
                throw new ArgumentNullException("fileSaveModel");
            }

            fileSaveModel.FileTimeStamp = DateTime.Now.ToUniversalTime();
            
            ViewDataUploadFileResult status;

            var dirInfo = new DirectoryInfo(fileSaveModel.StorageDirectory);
            var file = fileSaveModel.File;
            var fileNameWithoutPath = Path.GetFileName(fileSaveModel.File.FileName);
            var fileExtension = Path.GetExtension(fileNameWithoutPath);
            var fileName = Path.GetFileNameWithoutExtension(Path.GetFileName(fileSaveModel.File.FileName));
            var genName = fileName + "-" + fileSaveModel.FileTimeStamp.ToFileTime();
            var genFileName = genName + fileExtension;
            var fullPath = Path.Combine(fileSaveModel.StorageDirectory, genFileName);            

            try
            {                
                var viewDataUploadFileResult = new ViewDataUploadFileResult()
                {
                    Name = fileNameWithoutPath,
                    SavedFileName = genFileName,
                    Size = file.ContentLength,
                    FileType = file.ContentType,
                    Url = fileSaveModel.UrlPrefix + "/" + genFileName,
                    DeleteType = "POST",
                    Title = fileName,

                    // for controller use
                    FullPath = dirInfo.FullName + "/" + genFileName
                };

                // add delete url                           
                fileSaveModel.AddFileUriParamToDeleteUrl("fileUrl", viewDataUploadFileResult.Url);
                viewDataUploadFileResult.DeleteUrl = fileSaveModel.DeleteUrl;
                
                status = viewDataUploadFileResult;

                fileSaveModel.File.SaveAs(fullPath);
            }
            catch (Exception exc)
            {
                if (fileSaveModel.ThrowExceptions)
                {
                    throw;
                }

                status = new ViewDataUploadFileResult()
                             {
                                 Error = exc.Message,
                                 Name = file.FileName,
                                 Size = file.ContentLength,
                                 FileType = file.ContentType 
                             };
            }

            return status;
        }

        /// <summary>
        /// File saving logic to save files to DB and file system
        /// </summary>
        /// <param name="controller">Controller that contains data about uploaded files</param>
        /// <param name="deleteActionUrl">Url that will be used in file deletion</param>
        /// <param name="downloadActionUrl">Url that will be used in file downloading</param>
        /// <param name="attachmentType">Type of attachment block</param>
        /// <param name="attachmentLogic">Logic to have posibility save file information to DB</param>
        /// <param name="applicationId">Main application Id</param>
        /// <param name="attachmentBlock">Attachment block that contains data about all block files</param>
        public static ActionResult UploadFile(BaseController controller, string deleteActionUrl, string downloadActionUrl, AttachmentTypeEnum attachmentType, IAttachmentLogic attachmentLogic, int applicationId, AttachmentBlock attachmentBlock)
        {
            // here we can send in some extra info to be included with the delete url 
            var statuses = new List<ViewDataUploadFileResult>();
            for (var i = 0; i < controller.Request.Files.Count; i++)
            {
                var st = StoreFile(new FileSaveModel()
                {
                    File = controller.Request.Files[i],
                    // note how we are adding an additional value to be posted with delete request

                    // and giving it the same value posted with upload
                    DeleteUrl = controller.Url.Action(deleteActionUrl, new { attachmentType }),
                    StorageDirectory = controller.Server.MapPath("~/Content/uploads"),
                    UrlPrefix = ResolveServerUrl(VirtualPathUtility.ToAbsolute("~/Content/uploads"), false), // this is used to generate the relative url of the file

                    // overriding defaults
                    FileName = controller.Request.Files[i].FileName, // default is filename suffixed with filetimestamp
                    ThrowExceptions = true // default is false, if false exception message is set in error property
                });

                statuses.Add(st);
            }

            // statuses contains all the uploaded files details (if error occurs then check error property is not null or empty)
            // todo: add additional code to generate thumbnail for videos, associate files with entities etc

            // adding thumbnail url for jquery file upload javascript plugin
            statuses.ForEach(x => x.ThumbnailUrl = x.Url + string.Empty); // uses ImageResizer httpmodule to resize images from this url

            // setting custom download url instead of direct url to file which is default
            statuses.ForEach(x => x.Url = controller.Url.Action(downloadActionUrl, new { fileUrl = x.Url, mimetype = x.FileType }));

            var viewresult = controller.JsonPublic(new { files = statuses });
            // for IE8 which does not accept application/json
            if (controller.Request.Headers["Accept"] != null && !controller.Request.Headers["Accept"].Contains("application/json"))
            {
                viewresult.ContentType = "text/plain";
            }

            var attachments = new List<Attachment>();
            List<Attachment> newAttachments = null;

            if (attachmentBlock != null && attachmentBlock.Attachments != null)
            {
                newAttachments =
                    attachmentBlock.Attachments.Where(a => string.IsNullOrEmpty(a.ServerFileName)).ToList();
            }

            foreach (var viewDataUploadFileResult in statuses)
            {
                var idx = statuses.IndexOf(viewDataUploadFileResult);

                if (newAttachments != null && newAttachments.Count != 0)
                {
                    viewDataUploadFileResult.Description = newAttachments[idx].Description;
                    viewDataUploadFileResult.DocumentName = newAttachments[idx].DocumentName;
                    newAttachments[idx].ServerFileName = viewDataUploadFileResult.SavedFileName;
                }

                attachments.Add(new Attachment()
                {
                    FileName = viewDataUploadFileResult.Name,
                    ServerFileName = viewDataUploadFileResult.SavedFileName,
                    AttachmentType = attachmentType
                });
            }

            attachmentLogic.AddAttachments(attachmentType, applicationId, attachments, attachmentBlock);

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
        public static Uri ResolveServerUrl(string absolutetUrl, bool forceHttps)
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
        /// <param name="controller">Page Controller</param>
        /// <param name="fileUrl">
        /// The url to the file.
        /// </param>
        /// <param name="attachmentLogic">Logic for file deletion</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "need string"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "fileUrl", Justification = "need string"), HttpPost]
        public static ActionResult DeleteFile(BaseController controller, string fileUrl, IAttachmentLogic attachmentLogic)
        {
            var uri = new Uri(fileUrl);
            var filePath = controller.Server.MapPath(uri.LocalPath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            attachmentLogic.RemoveAttachment(new FileInfo(filePath).Name);

            var viewresult = controller.JsonPublic(new { error = string.Empty });
            // for IE8 which does not accept application/json
            if (controller.Request.Headers["Accept"] != null && !controller.Request.Headers["Accept"].Contains("application/json"))
            {
                viewresult.ContentType = "text/plain";
            }

            return viewresult; // trigger success
        }

        /// <summary>
        /// The download file function from file upload screen can be used some where else.
        /// </summary>
        /// <param name="controller">Page Controller</param>
        /// <param name="fileUrl">
        /// The file url for downloading.
        /// </param>
        /// <param name="mimetype">
        /// The mimetype.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "need string"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "fileUrl", Justification = "need string")]
        public static ActionResult DownloadFile(BaseController controller, string fileUrl, string mimetype)
        {
            var uri = new Uri(fileUrl);
            var filePath = controller.Server.MapPath(uri.LocalPath);

            if (File.Exists(filePath))
            {
                return controller.FilePublic(filePath, mimetype);
            }

            return new HttpNotFoundResult("File not found");
        }
    } 
}
