namespace Uma.Eservices.Logic.Features.OLE
{
    using System;
    using System.Linq;
    using Uma.Eservices.DbObjects.OLE.TableRefEnums;
    using Uma.Eservices.Logic.Features.FormCommonsMapper;
    using Uma.Eservices.Models.OLE;
    using db = Uma.Eservices.DbObjects.OLE;

    /// <summary>
    /// OLEOPIEducationInformationPageMapper obje mapper. Maps From Web to Db models and 
    /// From db to Web Models
    /// </summary>
    public static class OLEOPIEducationInformationPageMapper
    {
        #region From Db obj to Web Model obj

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEOPIEducationInformationPage db model</param>
        /// <returns>OLEOPIEducationInformationPage web model</returns>
        public static OLEOPIEducationInformationPage ToWebModel(this db.OLEOPIEducationInformationPage input)
        {
            if (input == null)
            {
                return new OLEOPIEducationInformationPage();
            }

            return new OLEOPIEducationInformationPage
            {
                ApplicationId = input.ApplicationId,
                PageId = input.Id,
                EducationInstitution = input.ToEducationInstitutionWebModel(),
                PreviousStudiesAndWork = input.ToPreviousStudiesAndWorkWebModel(),
                StayingLongerResoning = input.ToStayingLongerResoningWebModel()
            };
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEOPIEducationInformationPage db model</param>
        /// <returns>OLEOPIEducationInstitutionBlock web model</returns>
        public static OLEOPIEducationInstitutionBlock ToEducationInstitutionWebModel(this db.OLEOPIEducationInformationPage input)
        {
            if (input == null)
            {
                throw new ArgumentException("db.OLEOPIEducationInformationPage is null");
            }

            return new OLEOPIEducationInstitutionBlock
            {
                EducationalInstitution = input.EducationalInstitution,
                EducationalInstitutionName = input.EducationalInstitutionName,
                IsPresentAttendance = input.EducationIsPresentAttendance,
                LanguageOfStudy = input.EducationLanguageOfStudy,
                OtherLevelStudies = input.EducationOtherLevelStudies,
                OtherStudies = input.EducationOtherStudies,
                RegisterWhenInFinland = input.EducationRegisterWhenInFinland,
                StudyExchangeProgram = input.EducationStudyExchangeProgram,
                TermEndDate = input.EducationTermEndDate,
                TermStartDate = input.EducationTermStartDate,
                TypeOfStudies = input.EducationTypeOfStudies
            };
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEOPIEducationInformationPage db model</param>
        /// <returns>OLEOPIPreviousStudiesBlock web model</returns>
        public static OLEOPIPreviousStudiesBlock ToPreviousStudiesAndWorkWebModel(this db.OLEOPIEducationInformationPage input)
        {
            if (input == null)
            {
                throw new ArgumentException("db.OLEOPIEducationInformationPage is null");
            }

            return new OLEOPIPreviousStudiesBlock
            {
                PreviousStudies = input.PreviousStudies,
                PreviousStudiesConnectionToCurrent = input.PreviousStudiesConnectionToCurrent,
                WorkExperienceDescription = input.PreviousStudiesWorkExperienceDescription,
                WorkExperienceStatus = input.PreviousStudiesWorkExperienceStatus.ToWebModel()
            };
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEOPIEducationInformationPage db model</param>
        /// <returns>OLEOPIStayingBlock web model</returns>
        public static OLEOPIStayingBlock ToStayingLongerResoningWebModel(this db.OLEOPIEducationInformationPage input)
        {
            if (input == null)
            {
                throw new ArgumentException("db.OLEOPIEducationInformationPage is null");
            }

            return new OLEOPIStayingBlock
            {
                ApplicationId = input.ApplicationId,
                DurationOfStudies = input.StayingDurationOfStudies,
                ReasonToHaveLongerPermit = input.StayingReasonToHaveLongerPermit,
                ReasonToStayLonger = input.StayingReasonToStayLonger,
                ReasonToStudyInFinland = input.StayingReasonToStudyInFinland
            };
        }
        #endregion

        #region From Web Model obj to Db obj

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">OLEOPIEducationInformationPage Web model</param>
        /// <param name="objparam">OLEOPIEducationInformationPage object model</param>
        /// <returns>OLEOPIEducationInformationPage db model</returns>
        public static db.OLEOPIEducationInformationPage ToDbModel(this OLEOPIEducationInformationPage input, db.OLEOPIEducationInformationPage objparam)
        {
            if (input == null)
            {
                throw new ArgumentException("OLEOPIEducationInformationPage model is null");
            }
            objparam.ApplicationId = input.ApplicationId;

            ToEducationInstitutionDbModel(input.EducationInstitution, objparam);
            ToPreviousStudiesAndWorkDbModel(input.PreviousStudiesAndWork, objparam);
            ToStayingLongerResoningDbModel(input.StayingLongerResoning, objparam);

            return objparam;
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">OLEOPIStayingBlock Web model</param>
        /// <param name="dbModel">OLEOPIEducationInformationPage object model</param>
        private static void ToStayingLongerResoningDbModel(OLEOPIStayingBlock input, db.OLEOPIEducationInformationPage dbModel)
        {
            if (input == null || dbModel == null)
            {
                throw new ArgumentException("One of model is null");
            }

            dbModel.StayingDurationOfStudies = input.DurationOfStudies;
            dbModel.StayingReasonToHaveLongerPermit = input.ReasonToHaveLongerPermit;
            dbModel.StayingReasonToStayLonger = input.ReasonToStayLonger;
            dbModel.StayingReasonToStudyInFinland = input.ReasonToStudyInFinland;
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">OLEOPIPreviousStudiesBlock Web model</param>
        /// <param name="dbModel">OLEOPIEducationInformationPage object model</param>
        private static void ToPreviousStudiesAndWorkDbModel(OLEOPIPreviousStudiesBlock input, db.OLEOPIEducationInformationPage dbModel)
        {
            if (input == null || dbModel == null)
            {
                throw new ArgumentException("One of model is null");
            }

            dbModel.PreviousStudies = input.PreviousStudies;
            dbModel.PreviousStudiesConnectionToCurrent = input.PreviousStudiesConnectionToCurrent;
            dbModel.PreviousStudiesWorkExperienceDescription = input.WorkExperienceDescription;
            dbModel.PreviousStudiesWorkExperienceStatus = input.WorkExperienceStatus.ToDbModel();
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">OLEOPIEducationInstitutionBlock Web model</param>
        /// <param name="dbModel">OLEOPIEducationInformationPage object model</param>
        private static void ToEducationInstitutionDbModel(OLEOPIEducationInstitutionBlock input, db.OLEOPIEducationInformationPage dbModel)
        {
            if (input == null || dbModel == null)
            {
                throw new ArgumentException("One of model is null");
            }

            dbModel.EducationalInstitution = input.EducationalInstitution;
            dbModel.EducationalInstitutionName = input.EducationalInstitutionName;
            dbModel.EducationIsPresentAttendance = input.IsPresentAttendance;
            dbModel.EducationLanguageOfStudy = input.LanguageOfStudy;
            dbModel.EducationOtherLevelStudies = input.OtherLevelStudies;
            dbModel.EducationOtherStudies = input.OtherStudies;
            dbModel.EducationRegisterWhenInFinland = input.RegisterWhenInFinland;
            dbModel.EducationStudyExchangeProgram = input.StudyExchangeProgram;
            dbModel.EducationTermEndDate = input.TermEndDate;
            dbModel.EducationTermStartDate = input.TermStartDate;
            dbModel.EducationTypeOfStudies = input.TypeOfStudies;
        }

        #endregion
    }
}
