CREATE TABLE [dbo].[OriginalTextTranslation](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[OriginalTextId] [uniqueidentifier] NOT NULL,
	[Language] TINYINT NOT NULL,
	[Translation] [nvarchar](max) NOT NULL,
	CONSTRAINT [PK_OriginalTextTranslation] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_OriginalTextTranslation_OriginalText] FOREIGN KEY ([OriginalTextId]) REFERENCES [dbo].[OriginalText] ([Id]) ON DELETE CASCADE
)

GO
CREATE NONCLUSTERED INDEX [idxOriginalTextTranslation_Parent]
    ON [dbo].[OriginalTextTranslation]([OriginalTextId] ASC);
