namespace Uma.Eservices.Logic.Features.Common
{
    using System;
    using System.Linq;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Logic.Features.FormCommonsMapper;
    using Uma.Eservices.Logic.Features.OLE;
    using Uma.Eservices.Models.FormCommons;
    using db = Uma.Eservices.DbObjects;
    using dbCommon = Uma.Eservices.DbObjects.FormCommons;

    /// <summary>
    /// IFormsCommonLogic interface implementations
    /// </summary>
    public class FormsCommonLogic : IFormsCommonLogic
    {
        /// <summary>
        /// The holder for database helper
        /// </summary>
        private IGeneralDataHelper databaseHelper;

        /// <summary>
        /// Forms Logic contructor
        /// </summary>
        /// <param name="database">IGeneralDataHelper instance</param>
        public FormsCommonLogic(IGeneralDataHelper database)
        {
            this.databaseHelper = database;
        }

        /// <summary>
        /// Common method that is used to get necessary models for pdf creation proccess
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <returns>ApplicationFormModel model</returns>
        public ApplicationFormModel GetPDFModel(int applicationId)
        {

            db.ApplicationForm app = this.databaseHelper.Get<db.ApplicationForm>(o => o.ApplicationFormId == applicationId);
            ApplicationFormModel returnValue = new ApplicationFormModel();

            switch (app.FormCode)
            {
                case Uma.Eservices.DbObjects.FormCommons.FormType.Unknown:
                    break;
                case Uma.Eservices.DbObjects.FormCommons.FormType.OPIStudyResidencePermit:
                    // Set / map OLE OPI pages
                    returnValue.OleOpiPersonalInformationPge = app.OleOpiPersonalInformationPage.FirstOrDefault().ToWebModel();
                    returnValue.OleOpiEducationInformationPage = app.OleOpiEducationInformationPage.FirstOrDefault().ToWebModel();
                    returnValue.OleOpiFinancialInformationPage = app.OleOpiFinancialInformationPage.FirstOrDefault().ToWebModel();
                    break;
                default:
                    throw new ArgumentException("Unknown form type");
            }
            returnValue.FormCode = app.FormCode.ToWebModel();

            return returnValue;
        }

        /// <summary>
        /// Goes to database to check whether user already have application draft based on parameters - type and user id
        /// </summary>
        /// <param name="typeOfForm">The type of form to look for.</param>
        /// <returns>Id of existing Draft or NULL if application is not found</returns>
        public int? GetExistingApplicationId(FormType typeOfForm)
        {
            // TODO: probably needs parameters of application type and maybe user id and maybe something else...
            return null;
        }

        /// <summary>
        /// Creates new application for user in database and prefills it with necessary initial data.
        /// </summary>
        /// <param name="typeOfForm">The type of form to look for.</param>
        /// <param name="userId">The user Id</param>
        /// <returns>Unique ID of application</returns>
        public int CreateNewApplication(FormType typeOfForm, int userId)
        {
            // TODO: add parameters, actually create application structure, save it to DB, get its ID and return to caller.

            ApplicationForm applicationDbObject = null;

            switch (typeOfForm)
            {
                case FormType.Unknown:
                    break;
                case FormType.OPIStudyResidencePermit:
                    // ApplicationDbObject = this.databaseHelper.Get<ApplicationForm>(o => o.UserId == userId && o.FormCode == (short)FormType.OPIStudyResidencePermit);

                    if (applicationDbObject == null)
                    {
                        applicationDbObject = this.CreateApplicationForm(userId, typeOfForm);

                        this.databaseHelper.Create(applicationDbObject);
                        this.databaseHelper.FlushChanges();
                    }

                    break;
                default:
                    break;
            }

            return applicationDbObject.ApplicationFormId;
        }

        /// <summary>
        /// Changes for status enum valus to Submited
        /// </summary>
        /// <param name="appId">Application Form id</param>
        public void SubmitForm(int appId)
        {
            var application = this.databaseHelper.Get<ApplicationForm>(o => o.ApplicationFormId == appId);
            application.FormStatus = dbCommon.FormStatus.Submited;

            this.databaseHelper.Update<ApplicationForm>(application);
            this.databaseHelper.FlushChanges();
        }

        /// <summary>
        /// Returns default ApplicationForm object model
        /// </summary>
        /// <param name="userId">Unique User Id</param>
        /// <param name="formType">Application Form type</param>
        /// <returns>ApplicationForm model</returns>
        private ApplicationForm CreateApplicationForm(int userId, FormType formType)
        {
            var ftype = formType.ToDbModel();
            return new ApplicationForm()
            {
                FormCode = ftype,
                UserId = userId,
                FormStatus = 0,
                IsExtension = false
            };
        }
    }
}
