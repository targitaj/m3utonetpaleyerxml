namespace Uma.Eservices.LogicTests.OLE.ValidatorTests
{
    using FluentValidation.TestHelper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using FluentAssertions;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.OLE;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Logic.Features.OLE.OleValidators;
    using Uma.Eservices.Models.FormCommons;

    [TestClass]
    public class OLEContactInfoBlockValidatorTests
    {
        private OLEContactInfoBlockValidator validator;

        private OLEContactInfoBlock model;

        [TestInitialize]
        public void Init()
        {
            var locManager = new Mock<ILocalizationManager>();
            locManager.Setup(
                s => s.GetValidatorTranslationTEST(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Some test string");
            this.validator = new OLEContactInfoBlockValidator(locManager.Object);

            // Init test Model
            this.model = ClassPropertyInitializator.SetProperties<OLEContactInfoBlock>(new OLEContactInfoBlock());

            this.model.AddressInformation = new AddressInformation();
            this.model.AddressInformation = ClassPropertyInitializator.SetProperties<AddressInformation>(new AddressInformation());
        }

        [TestMethod]
        public void ValidateModel()
        {
            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(true);
        }

        [TestMethod]
        public void InvalidValidateModel()
        {
            this.model.TelephoneNumber = string.Empty;

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(1);
        }

        [TestMethod]
        public void TelephoneNumberNullOrEmptyTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.TelephoneNumber, (string)null);
            this.validator.ShouldHaveValidationErrorFor(o => o.TelephoneNumber, string.Empty);
        }

        [TestMethod]
        public void EmailAddressNullOrEmptyTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.EmailAddress, (string)null);
            this.validator.ShouldHaveValidationErrorFor(o => o.EmailAddress, string.Empty);
        }
    }
}
