namespace Uma.DataConnector.Contracts.Service
{
    using System.ServiceModel;
    using Uma.DataConnector.Contracts.Responses;

    /// <summary>
    /// Service Contract to get Master Data from UMA database
    /// </summary>
    [ServiceContract(Namespace = NS.ServiceNamespaceV1)]
    public interface IUmaMasterDataService
    {
        /// <summary>
        /// Pings the specified value.
        /// </summary>
        /// <param name="value">Value of no particular meaning. Supply anything you want.</param>
        /// <returns>Some string to check that service works</returns>
        [OperationContract]
        string Ping(long value);

        /// <summary>
        /// Gets the UMA CODE object by its LABEL value.
        /// </summary>
        /// <param name="codeLabel">The human readable identifier=LABEL of UMA CODE record.</param>
        /// <returns>Response with found CODE object values.</returns>
        [OperationContract]
        GetCodeResponse GetCodeByLabel(string codeLabel);

        /// <summary>
        /// Gets the UMA CODE object by its Identifier.
        /// </summary>
        /// <param name="codeId">Database unique identifier for Code record.</param>
        /// <returns>Response with found CODE object values.</returns>
        [OperationContract]
        GetCodeResponse GetCodeById(long codeId);

        /// <summary>
        /// Gets the List of UMA CODE-s which are still valid for given CODE_TYPE label.
        /// </summary>
        /// <param name="codeTypeLabel">The LABEL of CODE_TYPE.</param>
        /// <returns>List of CODE objects what are still valid and are referenced by given CODE_TYPE LABEL</returns>
        [OperationContract]
        GetCodeListResponse GetCodesByCodeTypeLabel(string codeTypeLabel);

        /// <summary>
        /// Reurns List of UMA STATE objects which are valid (ValidityDate checks)
        /// To be used in selections where list of countries is required.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "It's a gott damn operation!")]
        [OperationContract]
        GetCountryListResponse GetCountries();

        /// <summary>
        /// Reurns UMA STATE object by supplied ID. returns even invalid (by validation date) object
        /// To be used in prefilled data retrievals, where only state ID is stored.
        /// </summary>
        /// <param name="countryId">The country/STATE database unique identifier.</param>
        [OperationContract]
        GetCountryResponse GetCountryById(long countryId);
    }
}
