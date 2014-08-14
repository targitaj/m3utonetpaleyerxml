﻿CREATE TABLE [dbo].[OLEPersonalInformationPage]
(
	[Id] INT NOT NULL IDENTITY, 
    [ApplicationId] INT NULL, 
	[PersonalPersonNameFirstName] NVARCHAR(50) NULL, 
	[PersonalPersonNameLastName] NVARCHAR(50) NULL, 
	--[PersonalPreviousNames] INT NULL, 
    [PersonalGender] TINYINT NULL, 
    [PersonalBirthday] DATETIME NULL, 
    [PersonalPersonCode] NVARCHAR(50) NULL, 
    [PersonalBirthCountry] NVARCHAR(50) NULL, 
    [PersonalBirthPlace] NVARCHAR(50) NULL, 
    --[PersonalCurrentCitizenships] INT NULL, 
    --[PersonalPreviousCitizenships] INT NULL, 
    [PersonalMotherLanguage] NVARCHAR(50) NULL, 
    [PersonalCommunicationLanguage] TINYINT NULL, 
    [PersonalOccupation] NVARCHAR(50) NULL, 
    [PersonalEducation] NVARCHAR(50) NULL,
	--[ContactAddressInformation] INT NULL, 
    [ContactTelephoneNumber] NVARCHAR(50) NULL, 
    [ContactEmailAddress] NVARCHAR(50) NULL, 
    --[ContactFinlandAddressInformation] INT NULL, 
    [ContactFinlandTelephoneNumber] NVARCHAR(50) NULL, 
    [ContactFinlandEmailAddress] NVARCHAR(50) NULL, 
    [PassportPassportType] NVARCHAR(50) NULL, 
    [PassportPassportNumber] NVARCHAR(50) NULL, 
    [PassportInvalidPassport] BIT NULL, 
    [PassportIssuerCountry] NVARCHAR(50) NULL, 
    [PassportIssuerAuthority] NVARCHAR(50) NULL, 
    [PassportIssuedDate] DATETIME NULL, 
    [PassportExpirationDate] DATETIME NULL, 
    [ResidenceDurationAlreadyInFinland] BIT NULL, 
    [ResidenceDurationArrivalDate] DATETIME NULL, 
    [ResidenceDurationDepartDate] DATETIME NULL, 
    [ResidenceDurationDurationOfStay] NCHAR(50) NULL, 
    [FamilyStatus] SMALLINT NULL, 
    [FamilyPersonNameFirstName] NVARCHAR(50) NULL, 
	[FamilyPersonNameLastName] NVARCHAR(50) NULL, 
    --[FamilyPreviousNames] INT NULL, 
    [FamilyGender] TINYINT NULL,
    [FamilyBirthday] DATETIME NULL, 
    [FamilyPersonCode] NVARCHAR(50) NULL, 
    [FamilyBirthCountry] NVARCHAR(50) NULL, 
    [FamilyBirthPlace] NVARCHAR(50) NULL, 
    --[FamilyCurrentCitizenships] INT NULL, 
    [FamilySpouseIntentions] TINYINT NULL, 
    [FamilyHaveChildren] BIT NULL,
	--[FamilyChildren] INT NULL

	CONSTRAINT [PK_OLEPersonalInformationPage_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_OLEOPIPersonalInfo_ToEsrvApplication] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[ApplicationForm]([ApplicationFormId])
)