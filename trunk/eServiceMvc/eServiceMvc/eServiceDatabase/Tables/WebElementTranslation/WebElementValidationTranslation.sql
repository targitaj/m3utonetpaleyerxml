CREATE TABLE [dbo].[WebElementValidationTranslation]
(
	[TranslationId] INT NOT NULL IDENTITY , 
	[WebElementId] INT NOT NULL , 
	[Language] TINYINT NOT NULL,
	[TranslatedValidation] NVARCHAR(400) NOT NULL,
    CONSTRAINT [PK_WebElementValidationTranslation] PRIMARY KEY ([TranslationId]), 
    CONSTRAINT [FK_WebElementValidationTranslation_ToReasourceFeatures] FOREIGN KEY ([WebElementId]) REFERENCES [dbo].[WebElement]([WebElementId])
)

GO
CREATE NONCLUSTERED INDEX [idxWebElementValidationTranslation_Parent]
    ON [dbo].[WebElementValidationTranslation]([WebElementId] ASC);
