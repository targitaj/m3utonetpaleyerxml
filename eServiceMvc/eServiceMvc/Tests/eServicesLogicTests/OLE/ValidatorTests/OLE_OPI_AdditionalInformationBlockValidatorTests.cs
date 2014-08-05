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

    public class OLE_OPI_AdditionalInformationBlockValidatorTests
    {
        private OLEOPIAdditionalInformationBlockValidator validator;

        [TestInitialize]
        public void Init()
        {
            var locManager = new Mock<ILocalizationManager>();
            locManager.Setup(
                s => s.GetValidatorTranslationTEST(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Some test string");


            this.validator = new OLEOPIAdditionalInformationBlockValidator(locManager.Object);
        }

        [TestMethod]
        public void AdditionalInformationErrorTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.AdditionalInformation, (string)null);
            this.validator.ShouldHaveValidationErrorFor(o => o.AdditionalInformation, string.Empty);
        }

        [TestMethod]
        public void ModelMaxErrorCountTest()
        {
            var result = this.validator.Validate(new OLEOPIAdditionalInformationBlock());
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(1);
        }
    }
}
