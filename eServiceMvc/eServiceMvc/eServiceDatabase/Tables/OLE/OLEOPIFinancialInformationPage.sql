CREATE TABLE [dbo].[OLEOPIFinancialInformationPage]
(
	[Id] INT NOT NULL IDENTITY, 
	[ApplicationId] INT NULL, 

	[FinancialIncomeInfo]  TINYINT NULL, 
	[FinancialOtherIncome]  NVARCHAR(50) NULL, 
	[FinancialIsCurrentlyStudying]  BIT NULL, 
	[FinancialIsCurrentlyWorking]  BIT NULL, 
	[FinancialStudyWorkplaceName]  NVARCHAR(50) NULL, 

	 [HealthInsuredForAtLeastTwoYears] BIT NULL, 
    [HealthInsuredForLessThanTwoYears] BIT NULL, 
    [HealthHaveKelaCard] BIT NULL, 
    [HealthHaveEuropeanHealtInsurance] BIT NULL, 
    [AdditionalInformation] NVARCHAR(50) NULL, 
    [CriminalHaveCrimeConviction] BIT NULL, 
    [CriminalConvictionCrimeDescription] NVARCHAR(50) NULL, 
    [CriminalConvictionCountry] NVARCHAR(50) NULL, 
    [CriminalConvictionDate] DATETIME NULL, 
    [CriminalConvictionSentence] NVARCHAR(50) NULL, 
    [CriminalWasSuspectOfCrime] BIT NULL, 
    [CriminalCrimeAllegedOffence] NVARCHAR(50) NULL, 
    [CriminalCrimeCountry] NVARCHAR(50) NULL, 
    [CriminalCrimeDate] DATETIME NULL, 
    [CriminalRecordApproval] BIT NULL, 
    [CriminalRecordRetriveDenialReason] NVARCHAR(50) NULL, 
    [CriminalWasSchengenEntryRefusal] BIT NULL, 
    [CriminalSchengenEntryRefusalCountry] NVARCHAR(50) NULL, 
    [CriminalIsSchengenZoneEntryStillInForce] BIT NULL, 
    [CriminalSchengenEntryTimeRefusalExpiration] DATETIME NULL, 
    CONSTRAINT [PK_OLEOPIFinancialInformationPage_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_OLEOPIFinanciallInfo_ToEsrvApplication] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[ApplicationForm]([ApplicationFormId])
)
