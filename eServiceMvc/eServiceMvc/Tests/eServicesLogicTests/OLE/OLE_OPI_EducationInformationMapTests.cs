namespace Uma.Eservices.LogicTests.OLE
{
    using System.Collections.Generic;

    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Uma.Eservices.Models.OLE;
    using Uma.Eservices.Logic.Features.OLE;
    using Uma.Eservices.TestHelpers;
    using db = Uma.Eservices.DbObjects.OLE;
    using System.Diagnostics.CodeAnalysis;

    [TestClass]
    public class OLE_OPI_EducationInformationMapTests
    {
        private db.OLEOPIEducationInformationPage dbModel;
        private OLEOPIEducationInformationPage webModel;

        [TestInitialize]
        public void Init()
        {
            this.dbModel = ClassPropertyInitializator.SetProperties<db.OLEOPIEducationInformationPage>(new db.OLEOPIEducationInformationPage());
            this.dbModel.PreviousStudiesWorkExperienceStatus = db.OLEOPIWorkExperienceType.NotHaveexperience;


            this.webModel = ClassPropertyInitializator.SetProperties<OLEOPIEducationInformationPage>(new OLEOPIEducationInformationPage());
            this.webModel.EducationInstitution = ClassPropertyInitializator.SetProperties<OLEOPIEducationInstitutionBlock>(new OLEOPIEducationInstitutionBlock());
            this.webModel.StayingLongerResoning = ClassPropertyInitializator.SetProperties<OLEOPIStayingBlock>(new OLEOPIStayingBlock());
            this.webModel.PreviousStudiesAndWork = ClassPropertyInitializator.SetProperties<OLEOPIPreviousStudiesBlock>(new OLEOPIPreviousStudiesBlock());

            this.dbModel.Id = this.webModel.PageId;
        }

        #region To Web Model tests

        [TestMethod]
        public void ToEducationInfoWebModelTest()
        {
            var res = this.dbModel.ToWebModel();

            res.Should().NotBeNull();
            res.PageId.Should().Be(this.dbModel.Id);
            res.ApplicationId.Should().Be(this.dbModel.ApplicationId);
        }

        [TestMethod]
        public void ToEducationInstitutionWebModelTest()
        {
            var res = this.dbModel.ToWebModel().EducationInstitution;

            res.EducationalInstitution.Should().Be(this.dbModel.EducationalInstitution);
            res.EducationalInstitutionName.Should().Be(this.dbModel.EducationalInstitutionName);
            res.IsPresentAttendance.Should().Be(this.dbModel.EducationIsPresentAttendance);
            res.LanguageOfStudy.Should().Be(this.dbModel.EducationLanguageOfStudy);
            res.OtherLevelStudies.Should().Be(this.dbModel.EducationOtherLevelStudies);
            res.OtherStudies.Should().Be(this.dbModel.EducationOtherStudies);
            res.RegisterWhenInFinland.Should().Be(this.dbModel.EducationRegisterWhenInFinland);
            res.StudyExchangeProgram.Should().Be(this.dbModel.EducationStudyExchangeProgram);
            res.TermEndDate.Should().Be(this.dbModel.EducationTermEndDate);
            res.TermStartDate.Should().Be(this.dbModel.EducationTermStartDate);
            res.TypeOfStudies.Should().Be(this.dbModel.EducationTypeOfStudies);
        }


        [TestMethod]
        public void ToPreviousStudiesWebModelTest()
        {
            var res = this.dbModel.ToWebModel().PreviousStudiesAndWork;


            res.PreviousStudies.Should().Be(this.dbModel.PreviousStudies);
            res.PreviousStudiesConnectionToCurrent.Should().Be(this.dbModel.PreviousStudiesConnectionToCurrent);
            res.WorkExperienceDescription.Should().Be(this.dbModel.PreviousStudiesWorkExperienceDescription);
        }

        [TestMethod]
        public void ToStayingWebModelTest()
        {
            var res = this.dbModel.ToWebModel().StayingLongerResoning;

            res.DurationOfStudies.Should().Be(this.dbModel.StayingDurationOfStudies);
            res.ReasonToHaveLongerPermit.Should().Be(this.dbModel.StayingReasonToHaveLongerPermit);
            res.ReasonToStayLonger.Should().Be(this.dbModel.StayingReasonToStayLonger);
            res.ReasonToStudyInFinland.Should().Be(this.dbModel.StayingReasonToStudyInFinland);
        }

        [TestMethod]
        public void WorkExperienceTypeWebEnumTest()
        {
            var temp1 = db.OLEOPIWorkExperienceType.HaveExperience;
            var res1 = temp1.ToWebModel();
            res1.Should().Be(OLEOPIWorkExperienceType.HaveExperience);

            var temp2 = db.OLEOPIWorkExperienceType.NotHaveexperience;
            var res2 = temp2.ToWebModel();
            res2.Should().Be(OLEOPIWorkExperienceType.NotHaveexperience);

            var temp3 = db.OLEOPIWorkExperienceType.OtherWorkExperience;
            var res3 = temp3.ToWebModel();
            res3.Should().Be(OLEOPIWorkExperienceType.OtherWorkExperience);
        }

        #endregion

        #region To Db Model tests

        [TestMethod]
        public void ToEducationDbModelTest()
        {
            var res = this.webModel.ToDbModel(this.dbModel);

            res.ApplicationId.Should().Be(this.webModel.ApplicationId);
            res.Id.Should().Be(this.webModel.PageId);
        }

        [TestMethod]
        public void ToStayingLongerResoningDbModelTest()
        {
            var dbModel = this.webModel.ToDbModel(this.dbModel);

            dbModel.StayingDurationOfStudies.Should().Be(this.webModel.StayingLongerResoning.DurationOfStudies);
            dbModel.StayingReasonToHaveLongerPermit.Should().Be(this.webModel.StayingLongerResoning.ReasonToHaveLongerPermit);
            dbModel.StayingReasonToStayLonger.Should().Be(this.webModel.StayingLongerResoning.ReasonToStayLonger);
            dbModel.StayingReasonToStudyInFinland.Should().Be(this.webModel.StayingLongerResoning.ReasonToStudyInFinland);
        }

        [TestMethod]
        public void ToPreviousStudiesAndWorkDbModelTest()
        {
            var dbModel = this.webModel.ToDbModel(this.dbModel);

            dbModel.PreviousStudies.Should().Be(this.webModel.PreviousStudiesAndWork.PreviousStudies);
            dbModel.PreviousStudiesConnectionToCurrent.Should().Be(this.webModel.PreviousStudiesAndWork.PreviousStudiesConnectionToCurrent);
            dbModel.PreviousStudiesWorkExperienceDescription.Should().Be(this.webModel.PreviousStudiesAndWork.WorkExperienceDescription);
        }

        [TestMethod]
        public void ToEducationInstitutionDbModelTest()
        {
            var dbModel = this.webModel.ToDbModel(this.dbModel);

            dbModel.EducationalInstitution.Should().Be(this.webModel.EducationInstitution.EducationalInstitution);
            dbModel.EducationalInstitutionName.Should().Be(this.webModel.EducationInstitution.EducationalInstitutionName);
            dbModel.EducationIsPresentAttendance.Should().Be(this.webModel.EducationInstitution.IsPresentAttendance);
            dbModel.EducationLanguageOfStudy.Should().Be(this.webModel.EducationInstitution.LanguageOfStudy);
            dbModel.EducationOtherLevelStudies.Should().Be(this.webModel.EducationInstitution.OtherLevelStudies);
            dbModel.EducationOtherStudies.Should().Be(this.webModel.EducationInstitution.OtherStudies);
            dbModel.EducationRegisterWhenInFinland.Should().Be(this.webModel.EducationInstitution.RegisterWhenInFinland);
            dbModel.EducationStudyExchangeProgram.Should().Be(this.webModel.EducationInstitution.StudyExchangeProgram);
            dbModel.EducationTermEndDate.Should().Be(this.webModel.EducationInstitution.TermEndDate);
            dbModel.EducationTermStartDate.Should().Be(this.webModel.EducationInstitution.TermStartDate);
            dbModel.EducationTypeOfStudies.Should().Be(this.webModel.EducationInstitution.TypeOfStudies);
        }

        [TestMethod]
        public void WorkExperienceTypeDbEnumTest()
        {
            var temp1 = OLEOPIWorkExperienceType.HaveExperience;
            var res1 = temp1.ToDbModel();
            res1.Should().Be(db.OLEOPIWorkExperienceType.HaveExperience);

            var temp2 = OLEOPIWorkExperienceType.NotHaveexperience;
            var res2 = temp2.ToDbModel();
            res2.Should().Be(db.OLEOPIWorkExperienceType.NotHaveexperience);

            var temp3 = OLEOPIWorkExperienceType.OtherWorkExperience;
            var res3 = temp3.ToDbModel();
            res3.Should().Be(db.OLEOPIWorkExperienceType.OtherWorkExperience);
        }

        #endregion
    }
}

