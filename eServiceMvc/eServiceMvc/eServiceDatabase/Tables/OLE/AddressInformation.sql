CREATE TABLE [dbo].[AddressInformation]
(
	[Id] INT NOT NULL IDENTITY, 
	[OLEPersonalInformationPageId] INT NULL , 
    [StreetAddress] NVARCHAR(50) NULL, 
    [PostalCode] NVARCHAR(50) NULL, 
    [City] NVARCHAR(50) NULL, 
    [Country] NVARCHAR(50) NULL,
	[AddressInformationRefType] TINYINT NULL,

	CONSTRAINT [PK_AddressInformation_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AddressInformation_ToOLEPersonalInformationPage] FOREIGN KEY ([OLEPersonalInformationPageId]) REFERENCES [dbo].[OLEPersonalInformationPage]([Id])
)
