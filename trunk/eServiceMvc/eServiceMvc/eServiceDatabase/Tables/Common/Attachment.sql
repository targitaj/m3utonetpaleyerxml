CREATE TABLE [dbo].[Attachment]
(
	[Id] INT NOT NULL IDENTITY, 
	[ApplicationFormId] INT NOT NULL , 
    [AttachmentType] INT NOT NULL, 
	[Description] NVARCHAR(MAX) NULL, 
    [DocumentName] NVARCHAR(50) NULL,
	[FileName] NVARCHAR(100) NOT NULL, 
    [ServerFileName] NVARCHAR(150) NOT NULL

	CONSTRAINT [PK_Attachment_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_ApplicationFormId_ToApplicationForm] FOREIGN KEY ([ApplicationFormId]) REFERENCES [dbo].[ApplicationForm]([ApplicationFormId])
)
