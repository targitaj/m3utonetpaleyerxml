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
    public class OLEFamilyBlockValidatorTests
    {
        private OLEFamilyBlockValidator validator;

        private OLEFamilyBlock model;

        [TestInitialize]
        public void Init()
        {
            var locManager = new Mock<ILocalizationManager>();
            locManager.Setup(
                s => s.GetValidatorTranslationTEST(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Some test string");
            this.validator = new OLEFamilyBlockValidator(locManager.Object);

            // Init test Model
            this.model = ClassPropertyInitializator.SetProperties<OLEFamilyBlock>(new OLEFamilyBlock());
            this.model.PersonName = ClassPropertyInitializator.SetProperties<PersonName>(new PersonName());
            this.model.HaveChildren = false;
            this.model.Gender = Gender.Female;
            this.model.FamilyStatus = OLEFamilyStatus.Married;
            // this.validator.model = this.model;
        }

        [TestMethod]
        public void ValidFamiltyStatusTest()
        {
            this.validator.ShouldNotHaveValidationErrorFor(o => o.FamilyStatus, OLEFamilyStatus.Married);
            this.validator.ShouldNotHaveValidationErrorFor(o => o.FamilyStatus, OLEFamilyStatus.RegisteredRelationship);
            this.validator.ShouldNotHaveValidationErrorFor(o => o.FamilyStatus, OLEFamilyStatus.Widow);
        }

        [TestMethod]
        public void FamiltyStatusErrorTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.FamilyStatus, OLEFamilyStatus.Unspecified);
        }

        [TestMethod]
        public void ModelTest()
        {
            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(true);
        }

        /// <summary>
        /// This test will check that Validator
        // should not validate empt person name obj if IsInreations returns false
        /// </summary>
        [TestMethod]
        public void ModelTest2()
        {
            this.model.FamilyStatus = OLEFamilyStatus.Single;
            this.model.PersonName = new PersonName();

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(true);
        }

        /// <summary>
        /// This test will check that Validator
        /// should return errors (2), because personName propeties are null and IsInRelationShip is true 
        /// </summary>
        [TestMethod]
        public void ModelTest3()
        {
            this.model.FamilyStatus = OLEFamilyStatus.Married;
            this.model.PersonName = new PersonName();

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(2);
        }

        /// <summary>
        /// This should return errors, because
        /// property Model.HaveChildren == true, Children object should be validated
        /// </summary>
        [TestMethod]
        public void CildrenModelTest()
        {
            this.model.Children = new List<OLEChildData>();
            for (int i = 0; i < 3; i++)
            {
                var child = ClassPropertyInitializator.SetProperties<OLEChildData>(new OLEChildData());
                child.Birthday = DateTime.Now;
                child.Gender = Gender.Male;
                child.PersonCode = "1";
                this.model.Children.Add(child);
            }

            this.model.HaveChildren = true;

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(6);
            // 6 -> for Children model only Personcode and CurrentCitiz is missing ( 2 in total)
            // this.Model.Cildren.coun = 3 * 2 = 6
        }

        [TestMethod]
        public void CurrentCitizError()
        {
            this.model.CurrentCitizenships = new System.Collections.Generic.List<OLECurrentCitizenship>();
            for (int i = 0; i < 3; i++)
            {
                this.model.CurrentCitizenships.Add(new OLECurrentCitizenship());
            }

            this.model.HaveChildren = true;
            this.model.FamilyStatus = OLEFamilyStatus.Single;

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(3);
            // because model.currentCitizlist have 3 items each item contains one error
        }

        [TestMethod]
        public void GenderTest()
        {
            this.model.HaveChildren = true;
            this.model.FamilyStatus = OLEFamilyStatus.Single;
            this.model.Gender = Gender.NotSpecified;

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(1);
        }
    }
}
