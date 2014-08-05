namespace Uma.Eservices.DbAccess
{
    using System.Collections.Generic;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Interface for Locatization and translation related Data Access needs. 
    /// </summary>
    public interface ILocalizationDataHelper : IDataHelperBase
    {
        /// <summary>
        /// Gets all specified model element translations in all languages.
        /// </summary>
        /// <param name="modelName">Name of the model to retrieve.</param>
        /// <returns>List of View (r/o) objects with only necessary data</returns>
        List<ModelTranslation> GetAllModelTranslations(string modelName);
    }
}
