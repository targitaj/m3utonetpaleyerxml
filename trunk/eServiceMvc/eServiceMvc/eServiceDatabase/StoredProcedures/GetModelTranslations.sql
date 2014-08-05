CREATE PROCEDURE [dbo].[GetModelTranslations]
	@modelName varchar(50)
AS
	SELECT 
		elem.[PropertyName],
		trans.[TranslationType],
		trans.[Language],
		trans.[TranslatedText]
	FROM 
		[WebElement] elem
		LEFT JOIN [WebElementTranslation] trans ON trans.[WebElementId] = elem.[WebElementId]
	WHERE
		elem.ModelName = @modelName
RETURN 0
