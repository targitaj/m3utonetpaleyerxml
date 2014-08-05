namespace Uma.Eservices.Logic.Features.OLE
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.DbObjects.OLE.TableRefEnums;
    using Uma.Eservices.Logic.Features.Common;
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.Models.OLE;
    using Uma.Eservices.Models.Shared;
    using db = Uma.Eservices.DbObjects.OLE;
    using dbCommon = Uma.Eservices.DbObjects.FormCommons;

    /// <summary>
    /// Logic for working ole page with DB
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLE")]
    public partial class OLELogic : IOLELogic
    {
        /// <summary>
        /// The holder for database helper
        /// </summary>
        private IGeneralDataHelper databaseHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SandboxLogic" /> class.
        /// </summary>
        /// <param name="database">The database connection.</param>
        public OLELogic(IGeneralDataHelper database)
        {
            this.databaseHelper = database;
        }

        /// <summary>
        /// Retrieves data from database and composes model of first page of OLE applications
        /// </summary>
        /// <param name="id">ID of OLE application</param>
        /// <returns>Loaded, prefilled OLE Application first page model</returns>
        public OLEPersonalInformationPage GetPersonalInformationPageModel(int id)
        {
            var tempmodel = this.databaseHelper.Get<db.OLEPersonalInformationPage>(o => o.ApplicationId == id);

            if (tempmodel == null)
            {
                tempmodel = new db.OLEPersonalInformationPage();
                tempmodel.ApplicationId = id;
                this.databaseHelper.Create<db.OLEPersonalInformationPage>(tempmodel);
            }

            OLEPersonalInformationPage model = tempmodel.ToWebModel();

            model.FormProgress = this.GetFormProgressPrefill(FormType.OPIStudyResidencePermit);
            model.FormProgress.Pages[0].IsCurrent = true;
            return model;
        }

        /// <summary>
        /// Used for OLE attachment page creation
        /// </summary>
        /// <param name="id">Main application Id</param>
        public OLEAttachmentPage GetAttachmentPageModel(int id)
        {
            var att = databaseHelper.Get<ApplicationForm>(g => g.ApplicationFormId == id).Attachments;


            var model = new OLEAttachmentPage
            {
                ApplicationId = id,
                Certificate = CreateAttachmentBlockModel(id, AttachmentType.Certificate, att),
                Degree = CreateAttachmentBlockModel(id, AttachmentType.Degree, att),
                Guardian = CreateAttachmentBlockModel(id, AttachmentType.Guardian, att),
                EmploymentCertificates = CreateAttachmentBlockModel(id, AttachmentType.EmploymentCertificates, att),
                Health = CreateAttachmentBlockModel(id, AttachmentType.Health, att),
                Income = CreateAttachmentBlockModel(id, AttachmentType.Income, att),
                Refusal = CreateAttachmentBlockModel(id, AttachmentType.Refusal, att),
                Registration = CreateAttachmentBlockModel(id, AttachmentType.Registration, att),
                Supplemental = CreateAttachmentBlockModel(id, AttachmentType.Supplemental, att),
                Travel = CreateAttachmentBlockModel(id, AttachmentType.Travel, att)
            };
            model.FormProgress = this.GetFormProgressPrefill(FormType.OPIStudyResidencePermit);
            model.FormProgress.Pages[3].IsCurrent = true;
            return model;
        }

        /// <summary>
        /// Used for attachment block creation
        /// </summary>
        /// <param name="applicationId">Main aoolication Id</param>
        /// <param name="attachmentType">Attachment type</param>
        /// <param name="attachments">Db attachment list for conversion to model</param>
        private AttachmentBlock CreateAttachmentBlockModel(int applicationId, AttachmentType attachmentType, IEnumerable<dbCommon.Attachment> attachments)
        {
            return new AttachmentBlock()
                       {
                           ApplicationId = applicationId,
                           Attachments =
                               attachments.Where(a => a.AttachmentType == attachmentType)
                               .Select(s => s.ToWebModel())
                               .ToList()
                       };
        }

        /// <summary>
        /// Retrieves data from database and composes model of second page in OLE/OPI applications
        /// </summary>
        /// <param name="id">ID of OLE application</param>
        /// <returns>
        /// Loaded, prefilled OLE study Application second page model
        /// </returns>
        public OLEOPIEducationInformationPage GetEducationInformationPageModel(int id)
        {
            var tempmodel = this.databaseHelper.Get<db.OLEOPIEducationInformationPage>(o => o.ApplicationId == id);

            if (tempmodel == null)
            {
                tempmodel = new db.OLEOPIEducationInformationPage();
                tempmodel.ApplicationId = id;
                this.databaseHelper.Create<db.OLEOPIEducationInformationPage>(tempmodel);
            }

            OLEOPIEducationInformationPage model = tempmodel.ToWebModel();

            model.ApplicationId = id;
            model.FormProgress = this.GetFormProgressPrefill(FormType.OPIStudyResidencePermit);
            model.FormProgress.Pages[1].IsCurrent = true;
            return model;
        }

        /// <summary>
        /// Retrieves data from database and composes model of third page in OLE/OPI applications
        /// </summary>
        /// <param name="id">ID of OLE application</param>
        /// <returns>
        /// Loaded, prefilled OLE study Application third page model
        /// </returns>
        public OLEOPIFinancialInformationPage GetFinancialsInformationPageModel(int id)
        {
            var tempmodel = this.databaseHelper.Get<db.OLEOPIFinancialInformationPage>(o => o.ApplicationId == id);

            if (tempmodel == null)
            {
                tempmodel = new db.OLEOPIFinancialInformationPage();
                tempmodel.ApplicationId = id;
                this.databaseHelper.Create<db.OLEOPIFinancialInformationPage>(tempmodel);
            }

            OLEOPIFinancialInformationPage model = tempmodel.ToWebModel();

            model.ApplicationId = id;
            model.FormProgress = this.GetFormProgressPrefill(FormType.OPIStudyResidencePermit);
            model.FormProgress.Pages[2].IsCurrent = true;
            return model;
        }

        /// <summary>
        /// Retrieves data from database and composes model of data validation page of OLE OPI application
        /// </summary>
        /// <param name="id">ID of OLE application</param>
        /// <returns>Loaded, prefilled OLE Application validation page model</returns>
        public OLEOPIValidationPage GetValidationPageModel(int id)
        {
            // TODO: Actually do loading from database
            var model = new OLEOPIValidationPage
                            {
                                ApplicationId = id
                            };
            model.FormProgress = this.GetFormProgressPrefill(FormType.OPIStudyResidencePermit);
            model.FormProgress.Pages[4].IsCurrent = true;
            return model;
        }

        /// <summary>
        /// Retrieves data from database and composes model of sixth page of OLE applications
        /// </summary>
        /// <param name="id">ID of OLE application</param>
        /// <returns>Loaded, prefilled OLE Application sixth page model</returns>
        public OLEPaymentPage GetPaymentModel(int id)
        {
            // TO DO calculate and save payable amount to DB

            OLEPaymentPage model = new OLEPaymentPage();
            model.FormProgress = this.GetFormProgressPrefill(FormType.OPIStudyResidencePermit);
            model.FormProgress.Pages[5].IsCurrent = true;
            model.ApplicationId = id;
            return model;
        }

        /// <summary>
        /// Used for application submit information generation
        /// </summary>
        /// <param name="id">ID of OLE application</param>
        public OLEApplicationSubmit GetOLEApplicationSubmit(int id)
        {
            OLEApplicationSubmit model = new OLEApplicationSubmit();
            model.ApplicationId = id;
            model.FormProgress = this.GetFormProgressPrefill(FormType.OPIStudyResidencePermit);
            model.FormProgress.Pages[6].IsCurrent = true;

            return model;
        }

        #region

        /// <summary>
        /// Return application progress information depending on form type
        /// </summary>
        /// <param name="typeOfForm">Type of form</param>
        private FormProgressModel GetFormProgressPrefill(FormType typeOfForm)
        {
            switch (typeOfForm)
            {
                case FormType.OPIStudyResidencePermit:
                    return this.OLEOPIFormProgress;
            }
            return null;
        }

        /// <summary>
        /// Model for displaying overal form progress
        /// </summary>
        private FormProgressModel OLEOPIFormProgress
        {
            get
            {
                var formProgress = new FormProgressModel();
                formProgress.Pages = new List<PageStatus>(7);
                formProgress.Pages.Add(new PageStatus { Title = "Personal information" });
                formProgress.Pages.Add(new PageStatus { Title = "Studies" });
                formProgress.Pages.Add(new PageStatus { Title = "Income and insurance" });
                formProgress.Pages.Add(new PageStatus { Title = "Add documents" });
                formProgress.Pages.Add(new PageStatus { Title = "Review application" });
                formProgress.Pages.Add(new PageStatus { Title = "Pay" });
                formProgress.Pages.Add(new PageStatus { Title = "Submit" });
                return formProgress;
            }
        }
        #endregion
    }
}
