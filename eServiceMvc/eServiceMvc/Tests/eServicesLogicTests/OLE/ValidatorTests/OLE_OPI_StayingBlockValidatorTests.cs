namespace Uma.Eservices.LogicTests.OLE.ValidatorTests
{
    using System;

    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using FluentValidation.TestHelper;
    using Moq;

    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Logic.Features.OLE;
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.Models.OLE;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Logic.Features.OLE.OleValidators;
    using System.Collections.Generic;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;
    using System.Linq.Expressions;

    [TestClass]
    public class OLE_OPI_StayingBlockValidatorTests
    {
        private OLEOPIStayingBlockValidator validator;
        private Mock<IGeneralDataHelper> dbMock;
        private OLEOPIStayingBlock model;

        [TestInitialize]
        public void Init()
        {
            this.model = ClassPropertyInitializator.SetProperties<OLEOPIStayingBlock>(new OLEOPIStayingBlock());

            var locManager = new Mock<ILocalizationManager>();
            locManager.Setup(
                s => s.GetValidatorTranslationTEST(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Some test string");

            dbMock = new Mock<IGeneralDataHelper>();

            this.validator = new OLEOPIStayingBlockValidator(locManager.Object, dbMock.Object);
        }

        [TestMethod]
        public void DurationOfStudiesTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.DurationOfStudies, (string)null);
        }

        [TestMethod]
        public void ReasonToStudyInFinlandTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.ReasonToStudyInFinland, (string)null);
        }

        [TestMethod]
        public void ValidModelTest()
        {
            ApplicationForm appF = new ApplicationForm { IsExtension = true };
            this.dbMock.Setup(o => o.Get<ApplicationForm>(It.IsAny<Expression<Func<ApplicationForm, bool>>>()))
                .Returns<Expression<Func<ApplicationForm, bool>>>(predicate => appF);

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(true);

        }

        [TestMethod]
        public void ReasonToStudyInFinlandErrorTest()
        {
            ApplicationForm appF = new ApplicationForm { IsExtension = false };
            this.dbMock.Setup(o => o.Get<ApplicationForm>(It.IsAny<Expression<Func<ApplicationForm, bool>>>()))
                .Returns<Expression<Func<ApplicationForm, bool>>>(predicate => appF);

            this.model.ReasonToStudyInFinland = string.Empty;
            this.model.DurationOfStudies = string.Empty;
            var result = this.validator.Validate(this.model);

            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(2);
        }

        [TestMethod]
        public void ErrorCountTest()
        {
            ApplicationForm appF = new ApplicationForm { IsExtension = true };
            this.dbMock.Setup(o => o.Get<ApplicationForm>(It.IsAny<Expression<Func<ApplicationForm, bool>>>()))
                .Returns<Expression<Func<ApplicationForm, bool>>>(predicate => appF);
            this.model.DurationOfStudies = string.Empty;

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(1);
        }
    }
}
