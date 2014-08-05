namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using HtmlAgilityPack;
    using Uma.Eservices.Models.FormCommons;

    /// <summary>
    /// The uma ajax file upload for.
    /// </summary>
    public static partial class UmaAjaxHtmlHelpers
    {
        /// <summary>
        /// Returns File Upload control with posibility upload multiple files, with restrictions by file type, drag and drop, 
        /// image preview before upload and file max size check
        /// </summary>
        /// <param name="htmlHelper">The html helper.</param>
        /// <param name="maxFileSizeInBytes">
        /// The max File Size In Bytes.
        /// Remember add such things to web.config
        /// 
        /// <system.webServer>
        ///     <security>
        ///         <requestFiltering>
        ///             <requestLimits maxAllowedContentLength="2147483647"/>
        ///         </requestFiltering>
        ///     </security>
        /// </system.webServer>
        /// 
        /// and
        /// 
        /// <system.web>
        ///     <httpRuntime maxRequestLength="2147483647"/>
        /// </system.web>
        /// 
        /// </param>
        /// <param name="uploadUrl">The upload place where files will be stored.</param>
        /// <param name="sharedView">View that used for control html generation</param>
        /// <param name="model">Model from form</param>
        /// <param name="deleteUrl">Url that will be used in file deletion</param>
        /// <param name="downloadUrl">Url that will be used in file downloading</param>
        /// <param name="dataUploadTemplateId">Used to define template upload id if multiple upload forms used</param>
        /// <param name="dataDownloadTemplateId">Used to define template dounload id if multiple upload forms used</param>
        /// <param name="maxNumberOfFiles">Maxinmum number of files can be uploaded default unlimited</param>
        /// <param name="minNumberOfFiles">The Min Number Of Files that should be uploaded before elements will be enabled.</param>
        /// <param name="enablingIds">Elements that need to be disbaled untill min number of files are not uploaded</param>
        /// <param name="fileTypes">The file types alowed to upload.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#", Justification = "need as string")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "3#", Justification = "need as string")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "uploadUrl", Justification = "need as string")]
        public static MvcHtmlString UmaAjaxFileUpload(this HtmlHelper htmlHelper, long maxFileSizeInBytes, string uploadUrl, string sharedView, AttachmentBlock model, string deleteUrl, string downloadUrl, string dataUploadTemplateId = null, string dataDownloadTemplateId = null, int? maxNumberOfFiles = null, int? minNumberOfFiles = null, List<string> enablingIds = null, List<string> fileTypes = null)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException("htmlHelper");
            }

            if (string.IsNullOrEmpty(uploadUrl))
            {
                throw new ArgumentNullException("uploadUrl");
            }

            if (string.IsNullOrEmpty(sharedView))
            {
                throw new ArgumentNullException("sharedView");
            }

            if (!string.IsNullOrEmpty(dataUploadTemplateId) && string.IsNullOrEmpty(dataDownloadTemplateId))
            {
                throw new ArgumentNullException("dataDownloadTemplateId", "Defining dataUploadTemplateId need to define dataDownloadTemplateId");
            }

            if (string.IsNullOrEmpty(dataUploadTemplateId) && !string.IsNullOrEmpty(dataDownloadTemplateId))
            {
                throw new ArgumentNullException("dataUploadTemplateId", "Defining dataDownloadTemplateId need to define dataUploadTemplateId");
            }

            //var html = htmlHelper.Partial(sharedView, model).ToHtmlString();

            var html = UmaHtmlHelpers.GenerateHtmlFromPartialWithPrefix(htmlHelper, sharedView, model, "Attachments[{0}]", false);

            var fileTypeStr = (fileTypes == null || fileTypes.Count == 0)
                                  ? string.Empty
                                  : string.Format(@"acceptfiletypes='/(\.|\/)({0})$/i'", string.Join("|", fileTypes));

            var existingFilesVariable = "existingFilesVariable" + Guid.NewGuid().ToString().Replace("-", "");

            var attribHtml =
                string.Format(CultureInfo.InvariantCulture,
                    @"<div class='umaAjaxFileUpload' {0} maxfilesize='{1}' maxnumberoffiles='{2}' mimes='{3}' uploadurl='{4}' minnumberoffiles='{5}' enablingids='{6}' existingFilesVariable = '{7}'></div>",
                    fileTypeStr,
                    maxFileSizeInBytes,
                    maxNumberOfFiles,
                    GetMimes(fileTypes),
                    uploadUrl,
                    minNumberOfFiles,
                    enablingIds != null ? string.Join(",", enablingIds) : string.Empty,
                    existingFilesVariable);

            if (!string.IsNullOrEmpty(dataUploadTemplateId) && !string.IsNullOrEmpty(dataDownloadTemplateId))
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var nodes = doc.DocumentNode.SelectNodes("//script[@id]");
                foreach (HtmlNode input in nodes)
                {
                    HtmlAttribute att = input.Attributes["id"];
                    if (att.Value == "template-upload")
                    {
                        att.Value = dataUploadTemplateId;
                    }

                    if (att.Value == "template-download")
                    {
                        att.Value = dataDownloadTemplateId;
                    }
                }

                html = doc.DocumentNode.InnerHtml;
            }

            html = attribHtml + html;

            html += GenerateExistingVariableScript(htmlHelper, model, existingFilesVariable, deleteUrl);

            return MvcHtmlString.Create(html);
        }

        /// <summary>
        /// Generate script for existing attachments
        /// </summary>
        /// <param name="htmlHelper">The html helper.</param>
        /// <param name="model">Model with attachments</param>
        /// <param name="existingFilesVariable">Unique variable name</param>
        /// <param name="deleteUrl">Url for file deletion</param>
        private static string GenerateExistingVariableScript(HtmlHelper htmlHelper, AttachmentBlock model, string existingFilesVariable, string deleteUrl)
        {
            var res = "";

            if (model.Attachments != null && model.Attachments.Count != 0)
            {
                res = string.Format(@"<script>
var {0} = [", 
existingFilesVariable);

                foreach (var attachment in model.Attachments)
                {
                    var fileUrl = FileSaver.ResolveServerUrl(VirtualPathUtility.ToAbsolute("~/Content/uploads/"), false)
                                  + attachment.ServerFileName;

                    res += string.Format(@"{{
'Name': '{0}',
'DeleteUrl': '{1}',
'DeleteType': '{2}',
'Url': '{3}',
'Description' : '{4}',
'DocumentName' : '{5}',
'SavedFileName' : '{6}'}},
", 
 attachment.FileName,
 new UrlHelper(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection).Action(deleteUrl, new { attachmentType = attachment.AttachmentType, fileUrl }),
 "POST",
 fileUrl,
 attachment.Description,
 attachment.DocumentName,
 attachment.ServerFileName);
                }

                res += @"];


</script>";
            }

            return res;
        }

        /// <summary>
        /// Used for receiving mimes by file types.
        /// </summary>
        /// <param name="fileTypes">
        /// The file types that need convert to mimes.
        /// </param>
        private static string GetMimes(IEnumerable<string> fileTypes)
        {
            if (fileTypes == null || !fileTypes.Any())
            {
                return string.Empty;
            }

            var mimes = string.Empty;
            foreach (var fileType in fileTypes)
            {
                string mimeType = "application/unknown";
                Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("." + fileType);
                if (regKey != null && regKey.GetValue("Content Type") != null)
                {
                    mimeType = regKey.GetValue("Content Type").ToString();
                }

                if (!mimes.Contains(mimeType))
                {
                    mimes += mimeType + ",";
                }
            }

            return mimes.Substring(0, mimes.Length - 1);
        }
    }
}