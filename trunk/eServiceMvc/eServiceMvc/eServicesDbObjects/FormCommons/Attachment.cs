using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uma.Eservices.DbObjects.FormCommons
{
    using Uma.Eservices.DbObjects.OLE.TableRefEnums;

    /// <summary>
    /// Contains information about attachments
    /// </summary>
    public class Attachment
    {
        /// <summary>
        /// Person name object Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Contains ID valuue of Application to be preserved between web calls
        /// </summary>
        public int ApplicationFormId { get; set; }
        /// <summary>
        /// Description of an attacment
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Name of a document
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// Actual file name of attachment
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// File name which is stored(uploaded) on server
        /// </summary>
        public string ServerFileName { get; set; }

        /// <summary>
        /// Type of attachment
        /// </summary>
        public AttachmentType AttachmentType { get; set; }
    }
}
