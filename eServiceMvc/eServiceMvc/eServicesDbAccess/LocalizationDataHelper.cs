namespace Uma.Eservices.DbAccess
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Uma.Eservices.Common;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Class for database data access for localization and trenslation needs. 
    /// In addition to general data access methods usage (Get, GetMany, Count, Create, Update, Delete) it 
    /// contains localization-specific data retrieval methods
    /// </summary>
    public class LocalizationDataHelper : DataHelperBase, ILocalizationDataHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralDbDataHelper"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public LocalizationDataHelper(IUnitOfWork unitOfWork)
            : base(((UnitOfWork)unitOfWork).Context)
        {
        }

        /// <summary>
        /// Gets all specified model element translations in all languages.
        /// Note: returned translations are not tracked by EF, so changes to them will not be persisted
        /// </summary>
        /// <param name="modelName">Name of the model to retrieve.</param>
        /// <returns>List of View (r/o) objects with only necessary data</returns>
        [ExcludeFromCodeCoverage]
        public List<ModelTranslation> GetAllModelTranslations(string modelName)
        {
            // Using stored procedure as it is precompiled and fastest way of getting pre-fetched all particular model translations
            var result = this.DatabaseContext.Database.SqlQuery<ModelTranslation>("EXEC GetModelTranslations @modelName", new SqlParameter("modelName", modelName)).ToList();
            return result;
        }
    }
}
