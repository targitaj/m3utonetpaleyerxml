namespace Uma.Eservices.Logic.Features.Dashboard
{
    using System.Linq;
    using System.Collections.Generic;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.DbObjects.FormCommons;
    using Uma.Eservices.Models.Dashboard;
    using Uma.Eservices.Logic.Features.Common;

    /// <summary>
    /// DashboardLogic contains logic for managing dashboard
    /// </summary>
    public class DashboardLogic : IDashboardLogic
    {
        /// <summary>
        /// The holder for database helper
        /// </summary>
        private IGeneralDataHelper databaseHelper;

        /// <summary>
        /// Initialize instance of Dashboard
        /// </summary>
        /// <param name="database">Instance of database</param>
        public DashboardLogic(IGeneralDataHelper database)
        {
            this.databaseHelper = database;
        }

        /// <summary>
        /// Temporary solution to show and download pdf files
        /// </summary>
        /// <param name="userId">User Id for document loading</param>
        /// <returns>SupplementalDocumentModel model</returns>
        public SupplementalDocumentModel LoadDocuments(int userId)
        {
            var applicationDbObject = this.databaseHelper.Get<ApplicationForm>(o => o.UserId == userId);
            var pdfDoclist = this.databaseHelper.GetMany<Attachment>(o => o.AttachmentType == DbObjects.OLE.TableRefEnums.AttachmentType.PdfApplication).ToList();
            SupplementalDocumentModel returnVal = new SupplementalDocumentModel();
            returnVal.TempBool = false;

            returnVal.Attachments = new List<Models.FormCommons.Attachment>();
            foreach (Attachment item in pdfDoclist)
            {
                returnVal.Attachments.Add(item.ToWebModel());
            }

            return returnVal;
        }

        /// <summary>
        /// Creating initial model for supplemental document page
        /// </summary>
        public SupplementalDocumentModel SupplementalDocumentModel
        {
            get
            {
                var res = new SupplementalDocumentModel()
                              {
                                  Applications =
                                      new Dictionary<string, string>()
                                          {
                                              {
                                                  "Student_Resident_Permit",
                                                  "Student Resident Permit"
                                              }
                                          }
                              };

                return res;
            }
        }
    }
}
