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

    [TestClass]
    public class OLEPersonalDataBlockValidatorTests
    {
        private OLEPersonalDataBlockValidator validator;

        private OLEPersonalDataBlock model;

        [TestInitialize]
        public void Init()
        {
            var locManager = new Mock<ILocalizationManager>();
            locManager.Setup(
                s => s.GetValidatorTranslationTEST(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Some test string");
            this.validator = new OLEPersonalDataBlockValidator(locManager.Object);

            // Init test Model
            this.model = ClassPropertyInitializator.SetProperties<OLEPersonalDataBlock>(new OLEPersonalDataBlock());
            this.model.Birthday = DateTime.Now;
            this.model.Gender = Gender.Male;
            this.model.PersonCode = "1234";
            this.model.CommunicationLanguage = CommunicationLanguage.Finnish;
            this.model.PersonName = ClassPropertyInitializator.SetProperties<PersonName>(new PersonName());
            this.model.CurrentCitizenships = new System.Collections.Generic.List<OLECurrentCitizenship>();
            for (int i = 0; i < 3; i++)
            {
                this.model.CurrentCitizenships.Add(new OLECurrentCitizenship
                {
                    CurrentCitizenship = RandomData.GetString()
                });
            }
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
            this.validator.ShouldHaveValidationErrorFor(o => o.BirthCountry, (string)null);
            this.validator.ShouldHaveValidationErrorFor(o => o.BirthCountry, string.Empty);
        }

        [TestMethod]
        public void BirthPlaceTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.BirthPlace, (string)null);
            this.validator.ShouldHaveValidationErrorFor(o => o.BirthPlace, string.Empty);
        }

        [TestMethod]
        public void OccupationTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.Occupation, (string)null);
            this.validator.ShouldHaveValidationErrorFor(o => o.Occupation, string.Empty);
        }

        [TestMethod]
        public void EducationTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.Education, (string)null);
            this.validator.ShouldHaveValidationErrorFor(o => o.Education, string.Empty);
        }

        [TestMethod]
        public void MotherLangTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.MotherLanguage, (string)null);
            this.validator.ShouldHaveValidationErrorFor(o => o.MotherLanguage, string.Empty);
        }

        [TestMethod]
        public void ValidModelTest()
        {
            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(true);
        }

        [TestMethod]
        public void ModelValidationEmptyPeronName()
        {
            this.model.PersonName.FirstName = string.Empty;
            this.model.PersonName.LastName = string.Empty;

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);

            result.Errors.Count.Should().Be(2);
        }

        [TestMethod]
        public void ModelValidationCitizNullError()
        {
            this.model.CurrentCitizenships.ForEach(o => o.CurrentCitizenship = string.Empty);

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);

            result.Errors.Count.Should().Be(this.model.CurrentCitizenships.Count);
        }

    }
}
