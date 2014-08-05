CREATE TABLE [dbo].[OLEOPIEducationInformationPage]
(
	[Id] INT NOT NULL IDENTITY, 
	[ApplicationId] INT NULL, 

	[EducationalInstitution]  NVARCHAR(50) NULL, 
	[EducationalInstitutionName]  NVARCHAR(50) NULL, 
	[EducationTypeOfStudies]  NVARCHAR(50) NULL, 
	[EducationLanguageOfStudy]  NVARCHAR(50) NULL, 
	[EducationTermStartDate]  DATETIME NULL, 
	[EducationTermEndDate] DATETIME NULL, 
    [EducationIsPresentAttendance] BIT NULL, 
    [EducationRegisterWhenInFinland] BIT NULL,
	[EducationStudyExchangeProgram]  NVARCHAR(50) NULL, 
	[EducationOtherStudies]  NVARCHAR(50) NULL, 
	[EducationOtherLevelStudies]  NVARCHAR(50) NULL, 

	[StayingDurationOfStudies]  NVARCHAR(50) NULL, 
	[StayingReasonToStayLonger]  NVARCHAR(50) NULL, 
	[StayingReasonToHaveLongerPermit]  NVARCHAR(50) NULL, 
	[StayingReasonToStudyInFinland]  NVARCHAR(50) NULL, 

	[PreviousStudies]  NVARCHAR(50) NULL, 
	[PreviousStudiesConnectionToCurrent]  NVARCHAR(50) NULL, 
	[PreviousStudiesWorkExperienceStatus] TINYINT NULL, 
	[PreviousStudiesWorkExperienceDescription]  NVARCHAR(50) NULL, 

	 
    CONSTRAINT [PK_OLEOPIEducationInformationPage_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_OLEOPIEducationInfo_ToEsrvApplication] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[ApplicationForm]([ApplicationFormId])
)
