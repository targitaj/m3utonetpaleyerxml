namespace Uma.Eservices.Logic.Features.OLE
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Logic.Features.Common;
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.Models.OLE;
    using Uma.Eservices.Models.OLE.Enums;

    using db = Uma.Eservices.DbObjects.OLE;
    using fcdb = Uma.Eservices.DbObjects.FormCommons;

    /// <summary>
    /// OLE form save logic implementation
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLE")]
    public partial class OLELogic
    {
        /// <summary>
        /// Method saves Ole first page to DB
        /// </summary>
        /// <param name="model">OLEPersonalInformationPage model</param>
        public void SavePersonalInformationPageModel(OLEPersonalInformationPage model)
        {
            var dbModel = this.databaseHelper.Get<db.OLEPersonalInformationPage>(o => o.ApplicationId == model.ApplicationId);

            // Map Web model to DB model
            dbModel = model.ToDbModel(dbModel);
            this.databaseHelper.Update<db.OLEPersonalInformationPage>(dbModel);
        }

        /// <summary>
        /// Method saves Ole secound page to DB
        /// </summary>
        /// <param name="model">OLEOPIEducationInformationPage model</param>
        public void SaveEducationInformationPage(OLEOPIEducationInformationPage model)
        {
            var dbModel = this.databaseHelper.Get<db.OLEOPIEducationInformationPage>(o => o.ApplicationId == model.ApplicationId);

            // Map Web model to DB model
            dbModel = model.ToDbModel(dbModel);
            this.databaseHelper.Update(dbModel);
        }

        /// <summary>
        /// Method saves Ole third page to DB
        /// </summary>
        /// <param name="model">OLEOPIFinancialInformationPage model</param>
        public void SaveOLEOPIFinancialInformationPage(OLEOPIFinancialInformationPage model)
        {
            var dbModel = this.databaseHelper.Get<db.OLEOPIFinancialInformationPage>(o => o.ApplicationId == model.ApplicationId);

            // Map Web model to DB model
            dbModel = model.ToDbModel(dbModel);
            this.databaseHelper.Update(dbModel);
        }

        /// <summary>
        /// Adding attachment to DB
        /// </summary>
        /// <param name="type">Attachment type</param>
        /// <param name="applicationId">Main application id</param>
        /// <param name="attachments">List of new attachment that will be saved to db</param>
        /// <param name="attachmentBlock">Block that contains all attachment list</param>
        public void AddAttachments(AttachmentTypeEnum type, int applicationId, List<Attachment> attachments, AttachmentBlock attachmentBlock)
        {
            var dbModel = this.databaseHelper.Get<ApplicationForm>(o => o.ApplicationFormId == applicationId);

            dbModel.Attachments.AddRange(attachments.Select(s => s.ToDbModel(applicationId)));

            if (attachmentBlock != null && attachmentBlock.Attachments != null)
            {
                foreach (var attachment in attachmentBlock.Attachments)
                {
                    var att = dbModel.Attachments.FirstOrDefault(f => f.ServerFileName == attachment.ServerFileName);

                    if (att != null)
                    {
                        att.Description = attachment.Description;
                        att.DocumentName = attachment.DocumentName;
                    }
                }
            }

            this.databaseHelper.Update(dbModel);
        }

        /// <summary>
        /// Used for file deletion from db
        /// </summary>
        /// <param name="severFileName">File server name</param>
        public void RemoveAttachment(string severFileName)
        {
            var dbModel =
                this.databaseHelper.Get<ApplicationForm>(
                    o => o.Attachments.Any(s => s.ServerFileName == severFileName));

            dbModel.Attachments.Remove(dbModel.Attachments.First(a => a.ServerFileName == severFileName));

            this.databaseHelper.Update(dbModel);
        }
    }
}
