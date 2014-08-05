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

    [TestClass]
    public class OLE_OPI_EducationInstitutionBlockValidatorTests
    {
        private OLEOPIEducationInstitutionBlock model;

        private OLEOPIEducationInstitutionBlockValidator validator;

        [TestInitialize]
        public void Init()
        {
            this.model = ClassPropertyInitializator.SetProperties<OLEOPIEducationInstitutionBlock>(new OLEOPIEducationInstitutionBlock());

            var locManager = new Mock<ILocalizationManager>();
            locManager.Setup(
                s => s.GetValidatorTranslationTEST(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Some test string");
            this.validator = new OLEOPIEducationInstitutionBlockValidator(locManager.Object);
        }

        [TestMethod]
        public void EducationalInstitutionEmpty()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.EducationalInstitution, (string)null);
        }

        [TestMethod]
        public void TypeOfStudiesEmpty()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.TypeOfStudies, (string)null);
        }

        [TestMethod]
        public void TypeofStudiesTest()
        {
            var result = this.validator.Validate(this.model);
            this.model.TypeOfStudies = "DEGREE";
            this.model.IsPresentAttendance = true;
            result.IsValid.Should().Be(true);
        }

        [TestMethod]
        public void ErrorCountTest1()
        {
            this.model.TypeOfStudies = "DEGREE";
            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(1);
            // Because IsPresentAttendace is null
        }

        [TestMethod]
        public void IsPresentAttendanceTest()
        {
            this.model.EducationalInstitution = string.Empty;
            this.model.TypeOfStudies = "DEGREE";
            this.model.IsPresentAttendance = null;

            var result = this.validator.Validate(this.model);

            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(2);
            // Because All validation rules propeties are true, Except IsPresentAttendance
        }

        [TestMethod]
        public void TypeOfStudiesTest()
        {
            this.model.EducationalInstitution = string.Empty;
            this.model.TypeOfStudies = string.Empty;
            this.model.IsPresentAttendance = null;

            var result = this.validator.Validate(this.model);

            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(2);
            // Because All validation rules propeties are true, Except Type Of studies
        }
    }
}
