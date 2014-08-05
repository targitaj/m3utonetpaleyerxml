CREATE TABLE [dbo].[OLECitizenship]
(
	[Id] INT NOT NULL IDENTITY, 
	[OLEPersonalInformationPageId] INT NULL,
    [Citizenship] NVARCHAR(50) NULL,
	[CitizenshipRefType] TINYINT NULL,
	
	CONSTRAINT [PK_OLECitizenship_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_OLECitizenship_ToOLEPersonalInformationPage] FOREIGN KEY ([OLEPersonalInformationPageId]) REFERENCES [dbo].[OLEPersonalInformationPage]([Id])
)
