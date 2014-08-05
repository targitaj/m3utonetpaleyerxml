namespace Uma.Eservices.DbAccess
{
    using Uma.Eservices.Common;

    /// <summary>
    /// Class for general database data access. Basically for general data access methods usage (Get, GetMany, Count, Create, Update, Delete)
    /// </summary>
    public class GeneralDbDataHelper : DataHelperBase, IGeneralDataHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralDbDataHelper"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public GeneralDbDataHelper(IUnitOfWork unitOfWork)
            : base(((UnitOfWork)unitOfWork).Context)
        {
        }
    }
}
