namespace Uma.DataConnector.Mappers
{
    using Uma.DataConnector.Contracts.Data;
    using Uma.DataConnector.DAO;

    /// <summary>
    /// Mapping between CODE objects
    /// </summary>
    public static class CodeMapper
    {
        /// <summary>
        /// Creates WCF Data Contract CODE object from Database CODE object
        /// </summary>
        /// <param name="databaseObject">The database CODE object.</param>
        /// <returns>WCF Data Contract CODE object</returns>
        public static MasterDataCode DatabaseToContract(UmaCode databaseObject)
        {
            if (databaseObject == null)
            {
                return null;
            }

            return new MasterDataCode
                                   {
                                       CodeId = databaseObject.CodeId,
                                       Label = databaseObject.Label,
                                       Ordering = databaseObject.Ordering,
                                       TextEnglish = databaseObject.TextEnglish,
                                       TextFinnish = databaseObject.TextFinnish,
                                       TextSwedish = databaseObject.TextSwedish,
                                       CodeValue = databaseObject.CodeValue,
                                       Description = databaseObject.Description,
                                       KelaValue = databaseObject.KelaValue,
                                       ValidityStartDate = databaseObject.ValidityStartDate,
                                       ValidityEndDate = databaseObject.ValidityEndDate
                                   };
        }
    }
}