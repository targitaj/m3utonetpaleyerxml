CREATE TABLE [dbo].[FAQTranslations]
(
	[Id] INT NOT NULL IDENTITY, 
	[faqId] INT NOT NULL,
    [Question] NVARCHAR(150) NOT NULL, 
    [Answer] NVARCHAR(150) NOT NULL,
	[Language] TINYINT NOT NULL,
	CONSTRAINT [PK_FAQTranslations] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_FAQTranslations_ToFaqTranslations] FOREIGN KEY ([faqId]) REFERENCES [dbo].[FAQ]([Id])
)
