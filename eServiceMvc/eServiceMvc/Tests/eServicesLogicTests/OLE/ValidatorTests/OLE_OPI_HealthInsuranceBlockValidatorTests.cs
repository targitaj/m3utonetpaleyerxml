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
    public class OLE_OPI_HealthInsuranceBlockValidatorTests
    {
        private OLEOPIHealthInsuranceBlockValidator validator;
        private OLEOPIHealthInsuranceBlock model;

        [TestInitialize]
        public void Init()
        {
            this.model = new OLEOPIHealthInsuranceBlock();

            var locManager = new Mock<ILocalizationManager>();
            locManager.Setup(
                s => s.GetValidatorTranslationTEST(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Some test string");


            this.validator = new OLEOPIHealthInsuranceBlockValidator(locManager.Object);
        }

        [TestMethod]
        public void InsuredForAtLeastTwoYearsErrorTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.InsuredForAtLeastTwoYears, (bool?)null);
        }

        [TestMethod]
        public void InsuredForLessThanTwoYearsErrorTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.InsuredForLessThanTwoYears, (bool?)null);
        }

        [TestMethod]
        public void HaveKelaCardErrorTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.HaveKelaCard, (bool?)null);
        }

        [TestMethod]
        public void HaveEuropeanHealtInsuranceErrorTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.HaveEuropeanHealtInsurance, (bool?)null);
        }

        [TestMethod]
        public void MaxErrorTest()
        {
            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);

            result.Errors.Count.Should().Be(4);
        }

    }
}
