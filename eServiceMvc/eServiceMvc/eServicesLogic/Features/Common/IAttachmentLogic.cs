using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uma.Eservices.Logic.Features.Common
{
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.Models.OLE.Enums;

    /// <summary>
    /// Used for logic that contains attachment saving
    /// </summary>
    public interface IAttachmentLogic
    {
        /// <summary>
        /// Adding attachment to DB
        /// </summary>
        /// <param name="type">Attachment type</param>
        /// <param name="applicationId">Main application id</param>
        /// <param name="attachments">List of new attachment that will be saved to db</param>
        /// <param name="attachmentBlock">Block that contains all attachment list</param>
        void AddAttachments(AttachmentTypeEnum type, int applicationId, List<Attachment> attachments, AttachmentBlock attachmentBlock);

        /// <summary>
        /// Used for file deletion from db
        /// </summary>
        /// <param name="severFileName">File server name</param>
        void RemoveAttachment(string severFileName);
    }
}
