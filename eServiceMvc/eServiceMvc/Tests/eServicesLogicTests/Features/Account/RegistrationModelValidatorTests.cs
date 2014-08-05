namespace Uma.Eservices.LogicTests
{
    using FluentValidation.TestHelper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Uma.Eservices.Logic.Features.Account;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Account;
    using Uma.Eservices.TestHelpers;

    [TestClass]
    public class RegistrationModelValidatorTests
    {
        private RegistrationModelValidator validator;

        [TestInitialize]
        public void DefineValidator()
        {
            var locManager = new Mock<ILocalizationManager>();
            locManager.Setup(
                s => s.GetValidatorTranslationTEST(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Some test string");
            this.validator = new RegistrationModelValidator(locManager.Object);
        }

        [TestMethod]
        public void RegistrationModelValidatorChecksEmailForNull()
        {
            this.validator.ShouldHaveValidationErrorFor(l => l.Email, null as string);
        }

        [TestMethod]
        public void RegistrationModelValidatorChecksEmailForEmpty()
        {
            this.validator.ShouldHaveValidationErrorFor(l => l.Email, string.Empty);
        }

        [TestMethod]
        public void RegistrationModelValidatorChecksEmailValidityOnSimpleText()
        {
            this.validator.ShouldHaveValidationErrorFor(l => l.Email, "dummytext");
        }

        [TestMethod]
        public void RegistrationModelValidatorChecksEmailValidityOnIncompleteEmail1()
        {
            this.validator.ShouldHaveValidationErrorFor(l => l.Email, "dummy@text");
        }

        [TestMethod]
        public void RegistrationModelValidatorChecksEmailValidityOnIncompleteEmail2()
        {
            this.validator.ShouldHaveValidationErrorFor(l => l.Email, "dummy.text");
        }

        [TestMethod]
        public void RegistrationModelValidatorChecksEmailValidityOnIncompleteEmail3()
        {
            this.validator.ShouldHaveValidationErrorFor(l => l.Email, "dummy@text.");
        }

        [TestMethod]
        public void RegistrationModelValidatorChecksEmailValidityOnIncompleteEmail4()
        {
            this.validator.ShouldHaveValidationErrorFor(l => l.Email, "@.");
        }

        [TestMethod]
        public void RegistrationModelValidatorChecksEmailValidityOnValidEmail()
        {
            this.validator.ShouldNotHaveValidationErrorFor(l => l.Email, "me@my.pc");
        }

        [TestMethod]
        public void RegistrationModelValidatorChecksPasswordForNull()
        {
            this.validator.ShouldHaveValidationErrorFor(l => l.Password, null as string);
        }

        [TestMethod]
        public void RegistrationModelValidatorChecksPasswordForEmpty()
        {
            this.validator.ShouldHaveValidationErrorFor(l => l.Password, string.Empty);
        }

        [TestMethod]
        public void RegistrationModelValidatorChecksPasswordValid()
        {
            this.validator.ShouldNotHaveValidationErrorFor(l => l.Password, RandomData.GetString(8, 20, RandomData.StringIncludes.Lowercase | RandomData.StringIncludes.Numbers | RandomData.StringIncludes.Uppercase));
        }

        [TestMethod]
        public void RegistrationModelValidatorChecksPasswordConfirmForNull()
        {
            this.validator.ShouldHaveValidationErrorFor(l => l.PasswordConfirm, null as string);
        }

        [TestMethod]
        public void RegistrationModelValidatorChecksPasswordConfirmForEmpty()
        {
            this.validator.ShouldHaveValidationErrorFor(l => l.PasswordConfirm, string.Empty);
        }

        [TestMethod]
        public void RegistrationModelValidatorChecksPasswordsDoesntMatch1()
        {
            var model = new RegistrationViewModel { Password = "abcdefg", PasswordConfirm = "gfedcba", Email = "me@my.pc" };
            this.validator.ShouldHaveValidationErrorFor(l => l.PasswordConfirm, model);
        }

        [TestMethod]
        public void RegistrationModelValidatorChecksPasswordsDoesntMatch2()
        {
            var model = new RegistrationViewModel { Password = "abcdefg", PasswordConfirm = "ABCDEFG", Email = "me@my.pc" };
            this.validator.ShouldHaveValidationErrorFor(l => l.PasswordConfirm, model);
        }

        [TestMethod]
        public void RegistrationModelValidatorChecksPasswordsDoesntMatch3()
        {
            var model = new RegistrationViewModel { Password = "abcdefg", PasswordConfirm = "abcdefg ", Email = "me@my.pc" };
            this.validator.ShouldHaveValidationErrorFor(l => l.PasswordConfirm, model);
        }

        [TestMethod]
        public void RegistrationModelValidatorChecksPasswordsMatch()
        {
            var model = new RegistrationViewModel { Password = "NoSecret1$", PasswordConfirm = "NoSecret1$", Email = "me@my.pc" };
            this.validator.ShouldNotHaveValidationErrorFor(l => l.PasswordConfirm, model);
        }
    }
}
