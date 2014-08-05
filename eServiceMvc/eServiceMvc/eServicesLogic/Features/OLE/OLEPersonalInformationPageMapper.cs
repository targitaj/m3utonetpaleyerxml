namespace Uma.Eservices.Logic.Features.OLE
{
    using System;
    using System.Linq;
    using Uma.Eservices.DbObjects.OLE.TableRefEnums;
    using Uma.Eservices.Logic.Features.FormCommonsMapper;
    using Uma.Eservices.Models.OLE;
    using db = Uma.Eservices.DbObjects.OLE;

    /// <summary>
    /// OLEPersonalInformationPageMapper obje mapper. Maps From Web to Db models and 
    /// From db to Web Models
    /// </summary>
    public static class OLEPersonalInformationPageMapper
    {
        #region From Db obj to Web Model obj

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEPersonalInformationPage db model</param>
        /// <returns>OLEPersonalInformationPage web model</returns>
        public static OLEPersonalInformationPage ToWebModel(this db.OLEPersonalInformationPage input)
        {
            if (input == null)
            {
                return new OLEPersonalInformationPage();
            }

            return new OLEPersonalInformationPage
                {
                    PageId = input.Id,
                    ApplicationId = input.ApplicationId,
                    PersonDataBlock = input.ToOLEPersonalWebModel(),
                    ContactInformationBlock = input.ToOLEContactInfoWebModel(),
                    PassportInformationBlock = input.ToOLEPassportInformationWebModel(),
                    ResidenceDurationBlock = input.ToOLEResidenceDurationWebModel(),
                    FamilyBlock = input.ToOLEFamilyWebModel()
                };
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEPersonalInformationPage db model</param>
        /// <returns>OLEPersonalDataBlock web model</returns>
        public static OLEPersonalDataBlock ToOLEPersonalWebModel(this db.OLEPersonalInformationPage input)
        {
            if (input == null)
            {
                throw new ArgumentException(" db.OLEPersonalInformationPage model is null");
            }

            return new OLEPersonalDataBlock
            {
                BirthCountry = input.PersonalBirthCountry,
                Birthday = input.PersonalBirthday,
                BirthPlace = input.PersonalBirthPlace,
                CommunicationLanguage = input.PersonalCommunicationLanguage.ToWebModel(),

                CurrentCitizenships = input.OleCitizenShipList.Where(o => o.CitizenshipRefType == db.TableRefEnums.OLECitizenshipRefTypeEnum.OLEPersonalInformationPersonalCurrent)
                                                              .ToList()
                                                              .ToCurrentCitizWebModel(),

                Education = input.PersonalEducation,
                Gender = input.PersonalGender.ToWebModel(),
                MotherLanguage = input.PersonalMotherLanguage,
                Occupation = input.PersonalOccupation,
                PersonCode = input.PersonalPersonCode,
                PersonName = new Models.FormCommons.PersonName { FirstName = input.PersonalPersonNameFirstName, LastName = input.PersonalPersonNameLastName },

                PreviousCitizenships = input.OleCitizenShipList.Where(o => o.CitizenshipRefType == db.TableRefEnums.OLECitizenshipRefTypeEnum.OLEPersonalInformationPersonalPrevious)
                                                                .ToList()
                                                                .ToPreviousCitizWebModel(),

                PreviousNames = input.OlePersonNameList.Where(o => o.PersonNameRefType == db.TableRefEnums.PersonNameRefTypeEnum.OLEPersonalInformationPersonal)
                .ToList().ToWebModel()
            };
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEPersonalInformationPage db model</param>
        /// <returns>OLEPersonalDataBlockOLEContactInfoBlock web model</returns>
        public static OLEContactInfoBlock ToOLEContactInfoWebModel(this db.OLEPersonalInformationPage input)
        {
            if (input == null)
            {
                throw new ArgumentException(" db.OLEPersonalInformationPage model is null");
            }

            return new OLEContactInfoBlock
            {
                AddressInformation = input.OleAddressInformationList.Where(o => o.AddressInformationRefType == db.TableRefEnums.OLEAddressInformationRefTypeEnum.OLEPersonalInformationContactAddress)
                                                                            .FirstOrDefault().ToWebModel(),

                EmailAddress = input.ContactEmailAddress,
                FinlandAddressInformation = input.OleAddressInformationList.Where(o => o.AddressInformationRefType == db.TableRefEnums.OLEAddressInformationRefTypeEnum.OLEPersonalInformationContactFinlandAddress)
                                                                            .FirstOrDefault().ToWebModel(),

                FinlandEmailAddress = input.ContactFinlandEmailAddress,
                FinlandTelephoneNumber = input.ContactFinlandTelephoneNumber,
                // StateList
                TelephoneNumber = input.ContactTelephoneNumber
            };
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEPersonalInformationPage db model</param>
        /// <returns>OLEPassportInformationBlock web model</returns>
        public static OLEPassportInformationBlock ToOLEPassportInformationWebModel(this db.OLEPersonalInformationPage input)
        {
            if (input == null)
            {
                throw new ArgumentException(" db.OLEPersonalInformationPage model is null");
            }

            return new OLEPassportInformationBlock
            {
                ExpirationDate = input.PassportExpirationDate,
                InvalidPassport = input.PassportInvalidPassport,
                IssuedDate = input.PassportIssuedDate,
                IssuerAuthority = input.PassportIssuerAuthority,
                IssuerCountry = input.PassportIssuerCountry,
                PassportNumber = input.PassportPassportNumber,
                PassportType = input.PassportPassportType
                // StateList
            };
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEPersonalInformationPage db model</param>
        /// <returns>OLEResidenceDurationBlock web model</returns>
        public static OLEResidenceDurationBlock ToOLEResidenceDurationWebModel(this db.OLEPersonalInformationPage input)
        {
            if (input == null)
            {
                throw new ArgumentException(" db.OLEPersonalInformationPage model is null");
            }

            return new OLEResidenceDurationBlock
            {
                ApplicationId = input.ApplicationId,
                AlreadyInFinland = input.ResidenceDurationAlreadyInFinland,
                ArrivalDate = input.ResidenceDurationArrivalDate,
                DepartDate = input.ResidenceDurationDepartDate,
                DurationOfStay = input.ResidenceDurationDurationOfStay
            };
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEPersonalInformationPage db model</param>
        /// <returns>OLEFamilyBlock web model</returns>
        public static OLEFamilyBlock ToOLEFamilyWebModel(this db.OLEPersonalInformationPage input)
        {
            if (input == null)
            {
                throw new ArgumentException(" db.OLEPersonalInformationPage model is null");
            }

            return new OLEFamilyBlock
            {
                BirthCountry = input.FamilyBirthCountry,
                Birthday = input.FamilyBirthday,
                BirthPlace = input.FamilyBirthPlace,
                Children = input.OleChildDataList.Where(o => o.OLEChildDataRefType == db.TableRefEnums.OLEChildDataRefTypeEnum.OLEPersonalInformationFamilyChildren)
                                                        .ToList().ToWebModel(),

                CurrentCitizenships = input.OleCitizenShipList.Where(o => o.CitizenshipRefType == db.TableRefEnums.OLECitizenshipRefTypeEnum.OLEPersonalInformationFamilyCurrent)
                .ToList().ToCurrentCitizWebModel(),

                FamilyStatus = input.FamilyStatus.ToWebModel(),
                Gender = input.FamilyGender.ToWebModel(),
                HaveChildren = input.FamilyHaveChildren,
                PersonCode = input.FamilyPersonCode,
                PersonName = new Models.FormCommons.PersonName { FirstName = input.FamilyPersonNameFirstName, LastName = input.FamilyPersonNameLastName },

                PreviousNames = input.OlePersonNameList.Where(o => o.PersonNameRefType == db.TableRefEnums.PersonNameRefTypeEnum.OLEPersonalInformationFamily)
                                                                .ToList().ToWebModel(),

                SpouseIntentions = input.FamilySpouseIntentions.ToWebModel(),
                // StateList
            };
        }

        #endregion

        #region From Web Model obj to Db obj

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">OLEPersonalInformationPage Web model</param>
        /// <param name="objparam">OLEPersonalInformationPage object model</param>
        /// <returns>OLEPersonalInformationPage db model</returns>
        public static db.OLEPersonalInformationPage ToDbModel(this OLEPersonalInformationPage input, db.OLEPersonalInformationPage objparam)
        {
            if (input == null)
            {
                throw new ArgumentException("OLEPersonalInformationPage model is null");
            }

            ToOLEPersonalDbModel(input.PersonDataBlock, objparam);
            ToOLEContactInfoDbModel(input.ContactInformationBlock, objparam);
            ToOLEPassportInformationDbModel(input.PassportInformationBlock, objparam);
            ToOLEResidenceDurationDbModel(input.ResidenceDurationBlock, objparam);
            ToOLEFamilyDbModel(input.FamilyBlock, objparam);

            return objparam;
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">OLEPersonalDataBlock Web model</param>
        /// <param name="dbModel">OLEPersonalInformationPage object model</param>
        public static void ToOLEPersonalDbModel(OLEPersonalDataBlock input, db.OLEPersonalInformationPage dbModel)
        {
            if (input == null || dbModel == null)
            {
                throw new ArgumentException("One of model is null");
            }

            dbModel.PersonalBirthCountry = input.BirthCountry;
            dbModel.PersonalBirthday = input.Birthday;
            dbModel.PersonalBirthPlace = input.BirthPlace;
            dbModel.PersonalCommunicationLanguage = input.CommunicationLanguage.ToDbModel();
            dbModel.PersonalEducation = input.Education;
            dbModel.PersonalGender = input.Gender.ToDbModel();
            dbModel.PersonalMotherLanguage = input.MotherLanguage;
            dbModel.PersonalOccupation = input.Occupation;
            dbModel.PersonalPersonCode = input.PersonCode;
            dbModel.PersonalPersonNameFirstName = input.PersonName.FirstName;
            dbModel.PersonalPersonNameLastName = input.PersonName.LastName;

            input.CurrentCitizenships.ToCitizDbModel(OLECitizenshipRefTypeEnum.OLEPersonalInformationPersonalCurrent, dbModel.OleCitizenShipList);
            input.PreviousCitizenships.ToCitizDbModel(OLECitizenshipRefTypeEnum.OLEPersonalInformationPersonalPrevious, dbModel.OleCitizenShipList);
            input.PreviousNames.ToDbModel(db.TableRefEnums.PersonNameRefTypeEnum.OLEPersonalInformationPersonal, dbModel.OlePersonNameList);
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">OLEContactInfoBlock Web model</param>
        /// <param name="dbModel">OLEPersonalInformationPage object model</param>
        public static void ToOLEContactInfoDbModel(OLEContactInfoBlock input, db.OLEPersonalInformationPage dbModel)
        {
            if (input == null || dbModel == null)
            {
                throw new ArgumentException("One of model is null");
            }

            dbModel.ContactEmailAddress = input.EmailAddress;
            dbModel.ContactFinlandEmailAddress = input.FinlandEmailAddress;
            dbModel.ContactFinlandTelephoneNumber = input.FinlandTelephoneNumber;
            dbModel.ContactTelephoneNumber = input.TelephoneNumber;

            input.AddressInformation.ToDbModel(OLEAddressInformationRefTypeEnum.OLEPersonalInformationContactAddress, dbModel.OleAddressInformationList);
            input.FinlandAddressInformation.ToDbModel(OLEAddressInformationRefTypeEnum.OLEPersonalInformationContactFinlandAddress, dbModel.OleAddressInformationList);
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">OLEPassportInformationBlock Web model</param>
        /// <param name="dbModel">OLEPersonalInformationPage object model</param>
        public static void ToOLEPassportInformationDbModel(OLEPassportInformationBlock input, db.OLEPersonalInformationPage dbModel)
        {
            if (input == null || dbModel == null)
            {
                throw new ArgumentException("One of model is null");
            }

            dbModel.PassportExpirationDate = input.ExpirationDate;
            dbModel.PassportInvalidPassport = input.InvalidPassport;
            dbModel.PassportIssuedDate = input.IssuedDate;
            dbModel.PassportIssuerAuthority = input.IssuerAuthority;
            dbModel.PassportIssuerCountry = input.IssuerCountry;
            dbModel.PassportPassportNumber = input.PassportNumber;
            dbModel.PassportPassportType = input.PassportType;
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">OLEResidenceDurationBlock Web model</param>
        /// <param name="dbModel">OLEPersonalInformationPage object model</param>
        public static void ToOLEResidenceDurationDbModel(OLEResidenceDurationBlock input, db.OLEPersonalInformationPage dbModel)
        {
            if (input == null || dbModel == null)
            {
                throw new ArgumentException("One of model is null");
            }

            dbModel.ResidenceDurationAlreadyInFinland = input.AlreadyInFinland;
            dbModel.ResidenceDurationArrivalDate = input.ArrivalDate;
            dbModel.ResidenceDurationDepartDate = input.DepartDate;
            dbModel.ResidenceDurationDurationOfStay = input.DurationOfStay;
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">OLEFamilyBlock Web model</param>
        /// <param name="dbModel">OLEPersonalInformationPage object model</param>
        public static void ToOLEFamilyDbModel(OLEFamilyBlock input, db.OLEPersonalInformationPage dbModel)
        {
            if (input == null || dbModel == null)
            {
                throw new ArgumentException("One of model is null");
            }

            dbModel.FamilyBirthCountry = input.BirthCountry;
            dbModel.FamilyBirthday = input.Birthday;
            dbModel.FamilyBirthPlace = input.BirthPlace;
            dbModel.FamilyGender = input.Gender.ToDbModel();
            dbModel.FamilyHaveChildren = input.HaveChildren;
            dbModel.FamilyPersonCode = input.PersonCode;
            dbModel.FamilyPersonNameFirstName = input.PersonName.FirstName;
            dbModel.FamilyPersonNameLastName = input.PersonName.LastName;
            dbModel.FamilySpouseIntentions = input.SpouseIntentions.ToDbModel();
            dbModel.FamilyStatus = input.FamilyStatus.ToDbModel();

            input.Children.ToDbModel(OLEChildDataRefTypeEnum.OLEPersonalInformationFamilyChildren, dbModel.OleChildDataList);
            input.CurrentCitizenships.ToCitizDbModel(db.TableRefEnums.OLECitizenshipRefTypeEnum.OLEPersonalInformationFamilyCurrent, dbModel.OleCitizenShipList);
            input.PreviousNames.ToDbModel(db.TableRefEnums.PersonNameRefTypeEnum.OLEPersonalInformationFamily, dbModel.OlePersonNameList);
        }

        #endregion
    }
}
