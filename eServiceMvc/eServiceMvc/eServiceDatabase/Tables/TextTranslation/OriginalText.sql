CREATE TABLE [dbo].[OriginalText](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[Feature] [nvarchar](100) NOT NULL,
	[Original] [nvarchar](max) NOT NULL,
	CONSTRAINT [PK_OriginalText] PRIMARY KEY CLUSTERED 	([Id] ASC)
)