CREATE TABLE [dbo].[PersonName]
(
	[Id] INT NOT NULL IDENTITY, 
	[OLEPersonalInformationPageId] INT NULL, 
    [FirstName] NVARCHAR(50) NULL, 
    [LastName] NVARCHAR(50) NULL,
	[PersonNameRefType] TINYINT NULL,

	CONSTRAINT [PK_PersonName_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_PersonName_ToOLEPersonalInformationPage] FOREIGN KEY ([OLEPersonalInformationPageId]) REFERENCES [dbo].[OLEPersonalInformationPage]([Id])
)
