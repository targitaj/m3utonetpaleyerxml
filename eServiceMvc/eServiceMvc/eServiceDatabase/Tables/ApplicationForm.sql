CREATE TABLE [dbo].[ApplicationForm]
(
	[ApplicationFormId] INT NOT NULL IDENTITY(12000001,1), 
    [UserId] INT NOT NULL, 
	[IsExtension] BIT NOT Null,
    [FormCode] SMALLINT NOT NULL, 
    [FormStatus] SMALLINT NOT NULL,
	CONSTRAINT [PK_ApplicationForm_ApplicationFormId] PRIMARY KEY CLUSTERED ([ApplicationFormId] ASC),
	CONSTRAINT [FK_UserId_ToUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([UserId])
)
