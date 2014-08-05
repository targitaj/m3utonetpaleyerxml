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
    public class OLE_OPI_FinancialSupportBlockValidatorTests
    {
        private OLEOPIFinancialSupportBlockValidator validator;
        private OLEOPIFinancialSupportBlock model;

        [TestInitialize]
        public void Init()
        {
            this.model = ClassPropertyInitializator.SetProperties<OLEOPIFinancialSupportBlock>(new OLEOPIFinancialSupportBlock());
            this.model.IncomeInfo = OLEOPISupportTypes.JobEmployment;
            this.model.IsCurrentlyStudying = false;
            this.model.IsCurrentlyWorking = false;

            var locManager = new Mock<ILocalizationManager>();
            locManager.Setup(
                s => s.GetValidatorTranslationTEST(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Some test string");


            this.validator = new OLEOPIFinancialSupportBlockValidator(locManager.Object);
        }

        [TestMethod]
        public void IncomeInfoErrorTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.IncomeInfo, OLEOPISupportTypes.Unspecified);
        }

        [TestMethod]
        public void IncomeInfoValidValueTest()
        {
            this.validator.ShouldNotHaveValidationErrorFor(o => o.IncomeInfo, OLEOPISupportTypes.PersonalFunds);
        }

        [TestMethod]
        public void ValidModelTest()
        {
            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(true);
        }

        [TestMethod]
        public void OtherIncomeInfoErrorTest()
        {
            this.model.IncomeInfo = OLEOPISupportTypes.Other;
            this.model.OtherIncome = string.Empty;

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(1);
        }

        [TestMethod]
        public void StudyWorkplaceNameErrorTest1()
        {
            this.model.StudyWorkplaceName = string.Empty;
            this.model.IsCurrentlyStudying = true;
            this.model.IsCurrentlyWorking = null;

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(2);
        }

        [TestMethod]
        public void StudyWorkplaceNameErrorTest2()
        {
            this.model.StudyWorkplaceName = string.Empty;
            this.model.IsCurrentlyWorking = true;
            this.model.IsCurrentlyStudying = null;

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(2);
        }
    }
}
