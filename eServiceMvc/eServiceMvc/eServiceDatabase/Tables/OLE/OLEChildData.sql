CREATE TABLE [dbo].[OLEChildData]
(
	[Id] INT NOT NULL IDENTITY, 
	[OLEPersonalInformationPageId] INT NULL , 
    [PersonNameFirstName] NVARCHAR(50) NULL, 
	[PersonNameLastName] NVARCHAR(50) NULL, 
    [Gender] TINYINT NULL,
	[Birthday] DATETIME NULL, 
    [PersonCode] NVARCHAR(50) NULL, 
    [CurrentCitizenship] NVARCHAR(50) NULL, 
    [MigrationIntentions] TINYINT NULL,
	[OLEChildDataRefType] TINYINT NULL,

	CONSTRAINT [PK_ChildData_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_ChildData_ToOLEPersonalInformationPage] FOREIGN KEY ([OLEPersonalInformationPageId]) REFERENCES [dbo].[OLEPersonalInformationPage]([id])
)
