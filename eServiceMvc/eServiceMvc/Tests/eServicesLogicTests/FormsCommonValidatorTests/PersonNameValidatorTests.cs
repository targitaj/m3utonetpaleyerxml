namespace Uma.Eservices.LogicTests.FormsCommonValidatorTests
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using FluentValidation.TestHelper;
    using Moq;
    using Uma.Eservices.Logic.Features.FormCommonsValidator;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.TestHelpers;

    [TestClass]
    public class PersonNameValidatorTests
    {
        private PersonNameValidator validator;
        private PersonName model;

        [TestInitialize]
        public void Init()
        {
            var locManager = new Mock<ILocalizationManager>();
            locManager.Setup(
                s => s.GetValidatorTranslationTEST(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Some test string");
            this.validator = new PersonNameValidator(locManager.Object);

            this.model = ClassPropertyInitializator.SetProperties<PersonName>(new PersonName());
        }

        [TestMethod]
        public void PersonNameFirstNameNull()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.FirstName, (string)null);
        }

        [TestMethod]
        public void PersonNameFirstNameEmpty()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.FirstName, string.Empty);
        }

        [TestMethod]
        public void PersonNameLastNameNull()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.LastName, (string)null);
        }

        [TestMethod]
        public void PersonNameLastNameEmpty()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.LastName, string.Empty);
        }

        [TestMethod]
        public void ValidModelTest()
        {
            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(true);
        }

        [TestMethod]
        public void InValidModelTest()
        {
            this.model.FirstName = string.Empty;

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);

            result.Errors.Count.Should().Be(1);
        }
    }
}
