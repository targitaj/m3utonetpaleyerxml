CREATE TABLE [dbo].[FormDataOLE]
(
	[FormDataOLEId] INT NOT NULL IDENTITY(1000,1),
	[ApplicationFormId] INT NOT NULL,
	[PersLastName] NVARCHAR(100) NULL,
	[PersFirstName] NVARCHAR(100) NULL, 
	[PersGender] TINYINT NULL,
    [PersBirthday] DATE NULL, 
    [PersIdentityCode] VARCHAR(16) NULL, 
    [PersBirthCountry] VARCHAR(12) NULL, 
    [PersBirthPlace] NVARCHAR(200) NULL, 
    [PersMotherLanguage] VARCHAR(12) NULL, 
    [PersPreferredLanguage] TINYINT NULL, 
    [PersEducationLevel] NVARCHAR(12) NULL, 
    [PersProfessionalOccupation] NVARCHAR(200) NULL, 
    CONSTRAINT [PK_FormDataOLE_FormDataOLEId] PRIMARY KEY CLUSTERED ([FormDataOLEId] ASC)
)
