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
    public class OLEChildDataValidatorTests
    {
        private OLEChildDataValidator validator;

        private OLEChildData model;

        [TestInitialize]
        public void Init()
        {
            var locManager = new Mock<ILocalizationManager>();
            locManager.Setup(
                s => s.GetValidatorTranslationTEST(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Some test string");
            this.validator = new OLEChildDataValidator(locManager.Object);

            // Init test Model
            this.model = ClassPropertyInitializator.SetProperties<OLEChildData>(new OLEChildData());
            this.model.Birthday = DateTime.Now;
            this.model.Gender = Gender.Male;
            this.model.PersonCode = "1234";
            this.model.PersonName = ClassPropertyInitializator.SetProperties<PersonName>(new PersonName());
        }

        [TestMethod]
        public void GenderNotSpecifiedValueTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.Gender, Gender.NotSpecified);
        }

        [TestMethod]
        public void GenderValidValueTest()
        {
            this.validator.ShouldNotHaveValidationErrorFor(o => o.Gender, Gender.Male);
        }

        [TestMethod]
        public void BirthdayNullTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.Birthday, (DateTime?)null);
        }

        [TestMethod]
        public void PersconCodeNullOrEmptyTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.PersonCode, (string)null);
            this.validator.ShouldHaveValidationErrorFor(o => o.PersonCode, string.Empty);
        }

        [TestMethod]
        public void PersonCodeLenghtTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.PersonCode, RandomData.GetString(1, 3));
            this.validator.ShouldHaveValidationErrorFor(o => o.PersonCode, RandomData.GetString(5, 10));
            this.validator.ShouldNotHaveValidationErrorFor(o => o.PersonCode, RandomData.GetString(4, 4));
        }

        [TestMethod]
        public void BirthCountryTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.CurrentCitizenship, (string)null);
            this.validator.ShouldHaveValidationErrorFor(o => o.CurrentCitizenship, string.Empty);
        }

        [TestMethod]
        public void ValidateModelTest()
        {
            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(true);
        }

        [TestMethod]
        public void InValidateModelTest()
        {
            this.model.CurrentCitizenship = string.Empty;
            this.model.Gender = Gender.NotSpecified;

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(2);
        }
    }
}
