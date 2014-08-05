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
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;
    using System.Linq.Expressions;

    [TestClass]
    public class OLE_OPI_CiminalInfoBlockValidatorTests
    {
        private OLEOPICriminalInfoBlockValidator validator;
        private OLEOPICriminalInfoBlock model;

        [TestInitialize]
        public void Init()
        {
            this.model = ClassPropertyInitializator.SetProperties<OLEOPICriminalInfoBlock>(new OLEOPICriminalInfoBlock());
            this.model.HaveCrimeConviction = true;
            this.model.CriminalRecordApproval = true;
            this.model.WasSuspectOfCrime = true;
            this.model.IsSchengenZoneEntryStillInForce = true;
            this.model.WasSchengenEntryRefusal = true;

            // Date prop init
            this.model.ConvictionDate = RandomData.GetDateTime();
            this.model.CrimeDate = RandomData.GetDateTime();
            this.model.SchengenEntryTimeRefusalExpiration = RandomData.GetDateTime();

            var locManager = new Mock<ILocalizationManager>();
            locManager.Setup(
                s => s.GetValidatorTranslationTEST(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Some test string");


            this.validator = new OLEOPICriminalInfoBlockValidator(locManager.Object);
        }


        [TestMethod]
        public void HaveCrimeConvictionErrorTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.HaveCrimeConviction, (bool?)null);
        }

        [TestMethod]
        public void WasSuspectOfCrimeErrorTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.WasSuspectOfCrime, (bool?)null);
        }

        [TestMethod]
        public void CriminalRecordApprovalErrorTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.CriminalRecordApproval, (bool?)null);
        }

        [TestMethod]
        public void WasSchengenEntryRefusalErrorTest()
        {
            this.validator.ShouldHaveValidationErrorFor(o => o.WasSchengenEntryRefusal, (bool?)null);
        }

        [TestMethod]
        public void ValidModelTest()
        {
            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(true);
        }

        [TestMethod]
        public void HaveCriminalRelatedValidationTest()
        {
            this.model.ConvictionCrimeDescription = string.Empty;
            this.model.ConvictionCountry = string.Empty;
            this.model.ConvictionDate = null;
            this.model.ConvictionSentence = string.Empty;

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(4);
        }

        [TestMethod]
        public void WasSuspectOfCrimeRelatedValidationTest()
        {
            this.model.CrimeAllegedOffence = string.Empty;
            this.model.CrimeCountry = string.Empty;
            this.model.CrimeDate = null;

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(3);
        }

        [TestMethod]
        public void CriminalRecordApprovalRelatedValidationTest()
        {
            this.model.CriminalRecordRetriveDenialReason = string.Empty;

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(1);
        }

        [TestMethod]
        public void WasSchengenRelatedValidationTest()
        {
            this.model.SchengenEntryRefusalCountry = string.Empty;
            this.model.IsSchengenZoneEntryStillInForce = null;
            this.model.SchengenEntryTimeRefusalExpiration = null;

            var result = this.validator.Validate(this.model);
            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(3);
        }
    }
}
