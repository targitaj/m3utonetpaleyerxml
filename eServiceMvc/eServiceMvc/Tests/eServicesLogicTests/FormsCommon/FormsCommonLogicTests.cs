namespace Uma.Eservices.LogicTests.FormsCommon
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Linq.Expressions;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Logic.Features.Common;
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.TestHelpers;
    using dbCommon = Uma.Eservices.DbObjects.FormCommons;

    [TestClass]
    public class FormsCommonLogicTests
    {
        private Mock<IGeneralDataHelper> dbHelper;
        private FormsCommonLogic formsLogic;


        private ApplicationForm callbackVal;

        [TestInitialize]
        public void Init()
        {
            this.dbHelper = new Mock<IGeneralDataHelper>();
            this.dbHelper.Setup(o => o.Create(It.IsAny<ApplicationForm>())).Callback<ApplicationForm>(o => this.callbackVal = o);
            this.dbHelper.Setup(o => o.Update(It.IsAny<ApplicationForm>())).Callback<ApplicationForm>(o => this.callbackVal = o);
            this.dbHelper.Setup(o => o.Get(It.IsAny<Expression<Func<ApplicationForm, bool>>>()))
                .Returns(new ApplicationForm());

            this.formsLogic = new FormsCommonLogic(this.dbHelper.Object);
        }

        [TestMethod]
        public void CreateApplicationTest()
        {
            var res = this.formsLogic.CreateNewApplication(FormType.OPIStudyResidencePermit, RandomData.GetInteger(1, 111));
            this.callbackVal.FormStatus.Should().Be(dbCommon.FormStatus.Draft);
            this.dbHelper.Verify(o => o.Create(It.IsAny<ApplicationForm>()), Times.Once);
        }

        [TestMethod]
        public void SubmitFormTest()
        {
            this.formsLogic.SubmitForm(RandomData.GetInteger(1, 1000));
            this.callbackVal.FormStatus.Should().Be(dbCommon.FormStatus.Submited);
            this.dbHelper.Verify(o => o.Update(It.IsAny<ApplicationForm>()), Times.Once);

        }

        [TestMethod]
        public void GetPdfModelTest()
        {
            this.dbHelper.Setup(o => o.Get<ApplicationForm>(It.IsAny<Expression<Func<ApplicationForm, bool>>>()))
                .Returns(new ApplicationForm
                {
                    FormCode = dbCommon.FormType.OPIStudyResidencePermit,
                    OleOpiEducationInformationPage = new System.Collections.Generic.List<DbObjects.OLE.OLEOPIEducationInformationPage>(),
                    OleOpiFinancialInformationPage = new System.Collections.Generic.List<DbObjects.OLE.OLEOPIFinancialInformationPage>(),
                    OleOpiPersonalInformationPage = new System.Collections.Generic.List<DbObjects.OLE.OLEPersonalInformationPage>()
                });
            var result = this.formsLogic.GetPDFModel(RandomData.GetInteger(1, 1111));

            result.Should().NotBeNull();
            result.FormCode.Should().Be(FormType.OPIStudyResidencePermit);

            this.dbHelper.Verify(o => o.Get<ApplicationForm>(It.IsAny<Expression<Func<ApplicationForm, bool>>>()), Times.Once);
        }
    }
}
