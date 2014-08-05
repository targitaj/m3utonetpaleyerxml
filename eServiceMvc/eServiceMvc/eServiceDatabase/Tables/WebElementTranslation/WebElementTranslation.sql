CREATE TABLE [dbo].[WebElementTranslation]
(
	[TranslationId] INT NOT NULL IDENTITY , 
	[WebElementId] INT NOT NULL , 
    [TranslationType] TINYINT NOT NULL,
	[Language] TINYINT NOT NULL,
	[TranslatedText] NVARCHAR(400) NOT NULL,
    CONSTRAINT [PK_WebElementTranslation] PRIMARY KEY ([TranslationId]), 
    CONSTRAINT [FK_WebElementTranslation_ToReasourceFeatures] FOREIGN KEY ([WebElementId]) REFERENCES [dbo].[WebElement]([WebElementId])
)

GO
CREATE NONCLUSTERED INDEX [idxWebElementTranslation_Parent]
    ON [dbo].[WebElementTranslation]([WebElementId] ASC);
