namespace Uma.DataConnector.Mappers
{
    using Uma.DataConnector.Contracts.Data;
    using Uma.DataConnector.DAO;

    /// <summary>
    /// Mapping between STATE objects
    /// </summary>
    public static class StateMapper
    {
        /// <summary>
        /// Creates WCF Data Contract STATE object from Database STATE object
        /// </summary>
        /// <param name="databaseObject">The database STATE object.</param>
        /// <returns>WCF Data Contract STATE object</returns>
        public static MasterDataCountry DatabaseToContract(UmaState databaseObject)
        {
            if (databaseObject == null)
            {
                return null;
            }

            return new MasterDataCountry
            {
                StateId = databaseObject.StateId,
                Label = databaseObject.Label,
                NameEnglish = databaseObject.NameEnglish,
                NameFinnish = databaseObject.NameFinnish,
                NameSwedish = databaseObject.NameSwedish,
                NameNative = databaseObject.NameNative,
                NameBorder = databaseObject.NameBorder,
                GreaterArea = CodeMapper.DatabaseToContract(databaseObject.GreaterArea),
                ValidityStartDate = databaseObject.ValidityStartDate,
                ValidityEndDate = databaseObject.ValidityEndDate,
                ValidityExpired = databaseObject.ValidityExpired
            };
        }
    }
}