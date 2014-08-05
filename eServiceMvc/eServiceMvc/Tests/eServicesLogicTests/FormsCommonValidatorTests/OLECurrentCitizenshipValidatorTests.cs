namespace Uma.Eservices.LogicTests.FormsCommonValidatorTests
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
    using Uma.Eservices.Logic.Features.FormCommonsValidator;

    [TestClass]
    public class OLECurrentCitizenshipValidatorTests
    {
        private OLECurrentCitizenshipValidator validator;

        [TestInitialize]
        public void Init()
        {
            var locManager = new Mock<ILocalizationManager>();
            locManager.Setup(
                s => s.GetValidatorTranslationTEST(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Some test string");
            this.validator = new OLECurrentCitizenshipValidator(locManager.Object);
        }

        [TestMethod]
        public void ValidModelTest()
        {
            var result = this.validator.Validate(new OLECurrentCitizenship { CurrentCitizenship = RandomData.GetString() });
            result.IsValid.Should().Be(true);
        }

        [TestMethod]
        public void InvalidValidModelTest()
        {
            var result = this.validator.Validate(new OLECurrentCitizenship());
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(1);
        }
    }
}
