namespace Uma.DataConnector
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.ServiceModel;
    using Microsoft.Practices.Unity;
    using NHibernate;
    using Uma.DataConnector.Contracts.Responses;
    using Uma.DataConnector.Contracts.Service;
    using Uma.DataConnector.DAO;
    using Uma.DataConnector.Mappers;
    using Uma.Eservices.UmaConnWcf;
    using ILog = Uma.DataConnector.Logging.ILog;

    /// <summary>
    /// WCF Services to retrieve UMA master Data values to client
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class UmaMasterDataService : MarshalByRefObject, IUmaMasterDataService
    {
        /// <summary>
        /// Injected property for extensive Logging needs with NLOG.
        /// Getting configured through Web.Config NLOG section.
        /// Set here through Unity Container DI attribute and with specific extension(s) to give it a name
        /// </summary>
        [Dependency]
        public ILog Logger { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UmaMasterDataService"/> class.
        /// </summary>
        /// <param name="sessionFactoryParm">The NHibernate Session factory supplied through Unity Contaner constructor resolve.</param>
        public UmaMasterDataService(ISessionFactory sessionFactoryParm)
        {
            this.SessionFactory = sessionFactoryParm;
        }

        /// <summary>
        /// Gets or sets the reference NHibernate Session factory object, which is singleton through Unity container
        /// </summary>
        private ISessionFactory SessionFactory { get; set; }

        /// <summary>
        /// Check the workability of a Service. Returns passed value and Hash Sums for NHibernate Session factory and Session
        /// SessionFactory must stay the same, but Session must change!
        /// </summary>
        /// <param name="value">Value of no particular meaning. Supply anything you want.</param>
        /// <returns>Some string to check that service works</returns>
        public string Ping(long value)
        {
            // TODO: Refactor when monitoring tooling is created to return response contract with necessary environment variables
            ISession sesija = this.SessionFactory.GetCurrentSession();
            this.Logger.Info(
                "Called PING with value {0}. NHibernate Factory Hash: {1}, Session Hash: {2}",
                value.ToString("D"),
                sesija.SessionFactory.GetHashCode(),
                sesija.GetHashCode().ToString(CultureInfo.InvariantCulture));
            string sessIds = string.Concat("Session: ", sesija.GetHashCode().ToString(CultureInfo.InvariantCulture), " ; Factory: ", sesija.SessionFactory.GetHashCode());
            return string.Format(CultureInfo.CurrentCulture, "{1}. You submitted number {0}.", value, sessIds);
        }

        /// <summary>
        /// Gets the UMA CODE object by its LABEL value. Will return CODE record without checking its validity dates.
        /// </summary>
        /// <param name="codeLabel">The human readable identifier=LABEL of UMA CODE record.</param>
        /// <returns>Response with found CODE object values.</returns>
        [UmaConnTransaction]
        public GetCodeResponse GetCodeByLabel(string codeLabel)
        {
            var response = new GetCodeResponse();
            if (string.IsNullOrWhiteSpace(codeLabel))
            {
                response.OperationCallMessages.Add("Parameter - Label for CODE is either null or empty");
                return response;
            }

            string uppercaseLabel = codeLabel.ToUpperInvariant();
            try
            {
                ISession nhibernateSession = this.SessionFactory.GetCurrentSession();
                var umaCode = nhibernateSession.LinqQuery<UmaCode>().FirstOrDefault(code => code.Label == uppercaseLabel);
                if (umaCode == null)
                {
                    response.OperationCallMessages.Add(string.Format(CultureInfo.InvariantCulture, "Code by label {0} was not found in UMA", uppercaseLabel));
                }
                else
                {
                    response.Code = CodeMapper.DatabaseToContract(umaCode);
                    response.OperationCallStatus = CallStatus.Success;
                }
            }
            catch (Exception exc)
            {
                this.Logger.Error("UMA DB Query caused error", exc);
                response.OperationCallMessages.Add(exc.Message);
            }

            return response;
        }

        /// <summary>
        /// Gets the UMA CODE object by its Identifier.
        /// </summary>
        /// <param name="codeId">Database unique identifier for Code record.</param>
        /// <returns>Response with found CODE object values.</returns>
        [UmaConnTransaction]
        public GetCodeResponse GetCodeById(long codeId)
        {
            var response = new GetCodeResponse();
            try
            {
                ISession nhibernateSession = this.SessionFactory.GetCurrentSession();
                var umaCode = nhibernateSession.LinqQuery<UmaCode>().FirstOrDefault(code => code.CodeId == codeId);
                if (umaCode == null)
                {
                    response.OperationCallMessages.Add(string.Format(CultureInfo.InvariantCulture, "Code by Id {0} was not found in UMA", codeId.ToString("D", UmaConnCallContext.Current.ClientCulture)));
                }
                else
                {
                    response.Code = CodeMapper.DatabaseToContract(umaCode);
                    response.OperationCallStatus = CallStatus.Success;
                }
            }
            catch (Exception exc)
            {
                this.Logger.Error("UMA DB Query caused error", exc);
                response.OperationCallMessages.Add(exc.Message);
            }

            return response;
        }

        /// <summary>
        /// Gets the List of UMA CODE-s which are still valid for given CODE_TYPE label.
        /// Returns empty list if none found
        /// </summary>
        /// <param name="codeTypeLabel">The LABEL of CODE_TYPE.</param>
        /// <returns>List of CODE objects what are still valid and are referenced by given CODE_TYPE LABEL</returns>
        [UmaConnTransaction]
        public GetCodeListResponse GetCodesByCodeTypeLabel(string codeTypeLabel)
        {
            var response = new GetCodeListResponse();
            if (string.IsNullOrWhiteSpace(codeTypeLabel))
            {
                response.OperationCallMessages.Add("Parameter - Label for CODE_TYPE is either null or empty");
                return response;
            }

            string uppercaseLabel = codeTypeLabel.ToUpperInvariant();
            try
            {
                ISession nhibernateSession = this.SessionFactory.GetCurrentSession();
                var umaCodeType = nhibernateSession.LinqQuery<UmaCodeType>().FirstOrDefault(codeType => codeType.Label == uppercaseLabel);
                if (umaCodeType == null)
                {
                    response.OperationCallMessages.Add(string.Format(CultureInfo.InvariantCulture, "CodeType by label {0} was not found in UMA", uppercaseLabel));
                }
                else
                {
                    foreach (UmaCode umaCode in umaCodeType.Codes)
                    {
                        response.Codes.Add(CodeMapper.DatabaseToContract(umaCode));
                    }

                    response.OperationCallStatus = CallStatus.Success;
                }
            }
            catch (Exception exc)
            {
                this.Logger.Error("UMA DB Query caused error", exc);
                response.OperationCallMessages.Add(exc.Message);
            }

            return response;
        }

        /// <summary>
        /// Reurns List of UMA STATE objects which are valid (ValidityDate checks)
        /// To be used in selections where list of countries is required.
        /// </summary>
        [UmaConnTransaction]
        public GetCountryListResponse GetCountries()
        {
            var response = new GetCountryListResponse();
            try
            {
                ISession nhibernateSession = this.SessionFactory.GetCurrentSession();
                var umsStateList = nhibernateSession.LinqQuery<UmaState>().ToList();

                foreach (UmaState umaState in umsStateList)
                {
                    response.Countries.Add(StateMapper.DatabaseToContract(umaState));
                }

                response.OperationCallStatus = CallStatus.Success;
            }
            catch (Exception exc)
            {
                this.Logger.Error("UMA DB Query caused error", exc);
                response.OperationCallMessages.Add(exc.Message);
            }

            return response;
        }

        /// <summary>
        /// Reurns UMA STATE object by supplied ID. returns even invalid (by validation date) object
        /// To be used in prefilled data retrievals, where only state ID is stored.
        /// </summary>
        /// <param name="countryId">The country/STATE database unique identifier.</param>
        [UmaConnTransaction]
        public GetCountryResponse GetCountryById(long countryId)
        {
            var response = new GetCountryResponse();
            try
            {
                ISession nhibernateSession = this.SessionFactory.GetCurrentSession();
                var umaState = nhibernateSession.LinqQuery<UmaState>().FirstOrDefault(stt => stt.StateId == countryId);
                if (umaState == null)
                {
                    response.OperationCallMessages.Add(string.Format(CultureInfo.InvariantCulture, "State by Id {0} was not found in UMA", countryId.ToString("D", UmaConnCallContext.Current.ClientCulture)));
                }
                else
                {
                    response.Country = StateMapper.DatabaseToContract(umaState);
                    response.OperationCallStatus = CallStatus.Success;
                }
            }
            catch (Exception exc)
            {
                this.Logger.Error("UMA DB Query caused error", exc);
                response.OperationCallMessages.Add(exc.Message);
            }

            return response;
        }
    }
}
