namespace Uma.Eservices.LogicTests.OLE.ValidatorTests
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using FluentValidation.TestHelper;
    using Moq;

    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Logic.Features.FormCommonsValidator;

    [TestClass]
    public class AddressInformationValidatorTests
    {
        private AddressInformationValidator validator;
        private AddressInformation model;

        [TestInitialize]
        public void Init()
        {
            var locManager = new Mock<ILocalizationManager>();
            locManager.Setup(
                s => s.GetValidatorTranslationTEST(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Some test string");
            this.validator = new AddressInformationValidator(locManager.Object);

            // Init test Model
            this.model = ClassPropertyInitializator.SetProperties<AddressInformation>(new AddressInformation());
        }


        [TestMethod]
        public void StreetAddressTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.StreetAddress, (string)null);
            this.validator.ShouldHaveValidationErrorFor(o => o.StreetAddress, string.Empty);
        }


        [TestMethod]
        public void PostlaCodeTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.PostalCode, (string)null);
            this.validator.ShouldHaveValidationErrorFor(o => o.PostalCode, string.Empty);
        }


        [TestMethod]
        public void CityTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.City, (string)null);
            this.validator.ShouldHaveValidationErrorFor(o => o.City, string.Empty);
        }


        [TestMethod]
        public void CountryTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.Country, (string)null);
            this.validator.ShouldHaveValidationErrorFor(o => o.Country, string.Empty);
        }

        [TestMethod]
        public void ValidModelTest()
        {
            var result = this.validator.Validate(this.model);

            result.IsValid.Should().Be(true);
        }

        [TestMethod]
        public void ModelValidationErrorTest()
        {
            this.model.Country = null;
            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);

            result.Errors.Count.Should().Be(1);
        }
    }
}
