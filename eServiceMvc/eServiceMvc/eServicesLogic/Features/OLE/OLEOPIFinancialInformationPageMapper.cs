namespace Uma.Eservices.Logic.Features.OLE
{
    using System;
    using System.Linq;
    using Uma.Eservices.DbObjects.OLE.TableRefEnums;
    using Uma.Eservices.Logic.Features.FormCommonsMapper;
    using Uma.Eservices.Models.OLE;
    using db = Uma.Eservices.DbObjects.OLE;

    /// <summary>
    /// OLEOPIFinancialInformationPageMapper obje mapper. Maps From Web to Db models and 
    /// From db to Web Models
    /// </summary>
    public static class OLEOPIFinancialInformationPageMapper
    {
        #region From Db obj to Web Model obj

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEOPIFinancialInformationPage db model</param>
        /// <returns>OLEOPIFinancialInformationPage web model</returns>
        public static OLEOPIFinancialInformationPage ToWebModel(this db.OLEOPIFinancialInformationPage input)
        {
            if (input == null)
            {
                return new OLEOPIFinancialInformationPage();
            }

            return new OLEOPIFinancialInformationPage
            {
                ApplicationId = input.ApplicationId,
                PageId = input.Id,
                AdditionalInformation = new OLEOPIAdditionalInformationBlock { AdditionalInformation = input.AdditionalInformation },
                CriminalInformation = input.ToCriminalInformationWebModel(),
                FinancialStudySupport = input.ToFinancialStudySupportWebModel(),
                HealthInsurance = input.ToHealthInsuranceWebModel()
            };
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEOPIHealthInsuranceBlock db model</param>
        /// <returns>OLEOPIHealthInsuranceBlock web model</returns>
        public static OLEOPIHealthInsuranceBlock ToHealthInsuranceWebModel(this db.OLEOPIFinancialInformationPage input)
        {
            if (input == null)
            {
                throw new ArgumentException("db.OLEOPIEducationInformationPage is null");
            }

            return new OLEOPIHealthInsuranceBlock
            {
                HaveEuropeanHealtInsurance = input.HealthHaveEuropeanHealtInsurance,
                HaveKelaCard = input.HealthHaveKelaCard,
                InsuredForAtLeastTwoYears = input.HealthInsuredForAtLeastTwoYears,
                InsuredForLessThanTwoYears = input.HealthInsuredForLessThanTwoYears
            };
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEOPIFinancialSupportBlock db model</param>
        /// <returns>OLEOPIFinancialSupportBlock web model</returns>
        public static OLEOPIFinancialSupportBlock ToFinancialStudySupportWebModel(this db.OLEOPIFinancialInformationPage input)
        {
            if (input == null)
            {
                throw new ArgumentException("db.OLEOPIEducationInformationPage is null");
            }

            return new OLEOPIFinancialSupportBlock
            {
                IncomeInfo = input.FinancialIncomeInfo.ToWebModel(),
                IsCurrentlyStudying = input.FinancialIsCurrentlyStudying,
                IsCurrentlyWorking = input.FinancialIsCurrentlyWorking,
                OtherIncome = input.FinancialOtherIncome,
                StudyWorkplaceName = input.FinancialStudyWorkplaceName
            };
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEOPICriminalInfoBlock db model</param>
        /// <returns>OLEOPICriminalInfoBlock web model</returns>
        public static OLEOPICriminalInfoBlock ToCriminalInformationWebModel(this db.OLEOPIFinancialInformationPage input)
        {
            if (input == null)
            {
                throw new ArgumentException("db.OLEOPIEducationInformationPage is null");
            }

            return new OLEOPICriminalInfoBlock
            {
                ConvictionCountry = input.CriminalConvictionCountry,
                ConvictionCrimeDescription = input.CriminalConvictionCrimeDescription,
                ConvictionDate = input.CriminalConvictionDate,
                ConvictionSentence = input.CriminalConvictionSentence,
                CrimeAllegedOffence = input.CriminalCrimeAllegedOffence,
                CrimeCountry = input.CriminalCrimeCountry,
                CrimeDate = input.CriminalCrimeDate,
                CriminalRecordApproval = input.CriminalRecordApproval,
                CriminalRecordRetriveDenialReason = input.CriminalRecordRetriveDenialReason,
                HaveCrimeConviction = input.CriminalHaveCrimeConviction,
                IsSchengenZoneEntryStillInForce = input.CriminalIsSchengenZoneEntryStillInForce,
                SchengenEntryRefusalCountry = input.CriminalSchengenEntryRefusalCountry,
                SchengenEntryTimeRefusalExpiration = input.CriminalSchengenEntryTimeRefusalExpiration,
                WasSchengenEntryRefusal = input.CriminalWasSchengenEntryRefusal,
                WasSuspectOfCrime = input.CriminalWasSuspectOfCrime
            };
        }
        #endregion

        #region From Web Model obj to Db obj

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">OLEOPIFinancialInformationPage Web model</param>
        /// <param name="objparam">OLEOPIFinancialInformationPage object model</param>
        /// <returns>OLEOPIFinancialInformationPage db model</returns>
        public static db.OLEOPIFinancialInformationPage ToDbModel(this OLEOPIFinancialInformationPage input, db.OLEOPIFinancialInformationPage objparam)
        {
            if (input == null)
            {
                throw new ArgumentException("OLEOPIEducationInformationPage model is null");
            }

            objparam.AdditionalInformation = input.AdditionalInformation.AdditionalInformation;

            ToHealthInsuranceDbModel(input.HealthInsurance, objparam);
            ToFinancialStudySupportDbModel(input.FinancialStudySupport, objparam);
            ToCriminalInformationDbModel(input.CriminalInformation, objparam);

            return objparam;
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">OLEOPICriminalInfoBlock Web model</param>
        /// <param name="dbModel">OLEOPIFinancialInformationPage object model</param>
        private static void ToCriminalInformationDbModel(OLEOPICriminalInfoBlock input, db.OLEOPIFinancialInformationPage dbModel)
        {
            if (input == null || dbModel == null)
            {
                throw new ArgumentException("One of model is null");
            }

            dbModel.CriminalConvictionCountry = input.ConvictionCountry;
            dbModel.CriminalConvictionCrimeDescription = input.ConvictionCrimeDescription;
            dbModel.CriminalConvictionDate = input.ConvictionDate;
            dbModel.CriminalConvictionSentence = input.ConvictionSentence;
            dbModel.CriminalCrimeAllegedOffence = input.CrimeAllegedOffence;
            dbModel.CriminalCrimeCountry = input.CrimeCountry;
            dbModel.CriminalCrimeDate = input.CrimeDate;
            dbModel.CriminalHaveCrimeConviction = input.HaveCrimeConviction;
            dbModel.CriminalIsSchengenZoneEntryStillInForce = input.IsSchengenZoneEntryStillInForce;
            dbModel.CriminalRecordApproval = input.CriminalRecordApproval;
            dbModel.CriminalRecordRetriveDenialReason = input.CriminalRecordRetriveDenialReason;
            dbModel.CriminalSchengenEntryRefusalCountry = input.SchengenEntryRefusalCountry;
            dbModel.CriminalSchengenEntryTimeRefusalExpiration = input.SchengenEntryTimeRefusalExpiration;
            dbModel.CriminalWasSchengenEntryRefusal = input.WasSchengenEntryRefusal;
            dbModel.CriminalWasSuspectOfCrime = input.WasSuspectOfCrime;
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">OLEOPIFinancialSupportBlock Web model</param>
        /// <param name="dbModel">OLEOPIFinancialInformationPage object model</param>
        private static void ToFinancialStudySupportDbModel(OLEOPIFinancialSupportBlock input, db.OLEOPIFinancialInformationPage dbModel)
        {
            if (input == null || dbModel == null)
            {
                throw new ArgumentException("One of model is null");
            }

            dbModel.FinancialIncomeInfo = input.IncomeInfo.ToDbModel();
            dbModel.FinancialIsCurrentlyStudying = input.IsCurrentlyStudying;
            dbModel.FinancialIsCurrentlyWorking = input.IsCurrentlyWorking;
            dbModel.FinancialOtherIncome = input.OtherIncome;
            dbModel.FinancialStudyWorkplaceName = input.StudyWorkplaceName;
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">OLEOPIStayingBlock Web model</param>
        /// <param name="dbModel">OLEOPIFinancialInformationPage object model</param>
        private static void ToHealthInsuranceDbModel(OLEOPIHealthInsuranceBlock input, db.OLEOPIFinancialInformationPage dbModel)
        {
            if (input == null || dbModel == null)
            {
                throw new ArgumentException("One of model is null");
            }

            dbModel.HealthHaveEuropeanHealtInsurance = input.HaveEuropeanHealtInsurance;
            dbModel.HealthHaveKelaCard = input.HaveKelaCard;
            dbModel.HealthInsuredForAtLeastTwoYears = input.InsuredForAtLeastTwoYears;
            dbModel.HealthInsuredForLessThanTwoYears = input.InsuredForLessThanTwoYears;
        }

        #endregion
    }
}
