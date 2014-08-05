CREATE TABLE [dbo].WebElement
(
	[WebElementId] INT NOT NULL IDENTITY, 
    [ModelName] NVARCHAR(50) NOT NULL, 
	[PropertyName] NVARCHAR(50) NOT NULL,
    CONSTRAINT [PK_WebElement] PRIMARY KEY ([WebElementId])
)

GO
CREATE NONCLUSTERED INDEX [idxWebElement_ModelName]
    ON [dbo].[WebElement]([ModelName] ASC);
