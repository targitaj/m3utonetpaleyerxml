namespace Uma.Eservices.LogicTests
{
    using FluentValidation.TestHelper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Uma.Eservices.Logic.Features.Account;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Localization;

    [TestClass]
    public class LoginModelValidatorTests
    {
        private LoginModelValidator validator;

        [TestInitialize]
        public void DefineValidator()
        {
            var locManager = new Mock<ILocalizationManager>();
            locManager.Setup(
                s => s.GetValidatorTranslationTEST(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Some test string");
            this.validator = new LoginModelValidator(locManager.Object);
        }

        [TestMethod]
        public void LoginModelValidatorChecksEmailForNull()
        {
            this.validator.ShouldHaveValidationErrorFor(l => l.Email, null as string);
        }

        [TestMethod]
        public void LoginModelValidatorChecksEmailForEmpty()
        {
            this.validator.ShouldHaveValidationErrorFor(l => l.Email, string.Empty);
        }

        [TestMethod]
        public void LoginModelValidatorChecksPasswordForNull()
        {
            this.validator.ShouldHaveValidationErrorFor(l => l.Password, null as string);
        }

        [TestMethod]
        public void LoginModelValidatorChecksPasswordForEmpty()
        {
            this.validator.ShouldHaveValidationErrorFor(l => l.Password, string.Empty);
        }
    }
}
