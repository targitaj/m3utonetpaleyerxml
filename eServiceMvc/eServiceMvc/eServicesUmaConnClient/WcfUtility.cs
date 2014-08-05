namespace Uma.Eservices.UmaConnClient
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.ServiceModel;

    /// <summary>
    /// Helper class to create WCF Channel Factory (used by IoC container)
    /// </summary>
    public static class WcfUtility
    {
        #region WCF Service Reader default values override constants
        /// <summary>
        /// The WCF serializer maximum string content length to 16MB (default is 8192)
        /// </summary>
        private const int WcfSerializerMaxStringContentLength = 16 * 1024 * 1024;

        /// <summary>
        /// The WCF serializer maximum array length to 65535/16bit (default is 16384)
        /// </summary>
        private const int WcfSerializerMaxArrayLength = 65535;

        /// <summary>
        /// The WCF serializer maximum bytes per read to 16MB (default is 4096)
        /// </summary>
        private const int WcfSerializerMaxBytesPerRead = 16 * 1024 * 1024;

        /// <summary>
        /// The WCF serializer maximum depth to 65535/16bit (default is 32)
        /// </summary>
        private const int WcfSerializerMaxDepth = 65535;

        /// <summary>
        /// The WCF serializer maximum name table character count as default
        /// </summary>
        private const int WcfSerializerMaxNameTableCharCount = 16384;
        #endregion

        /// <summary>
        /// Gets a channel factory for the given service type
        /// </summary>
        /// <summary>Retrieves a WCF Client for the T interface.</summary>
        /// <typeparam name="T">The Interface the WCF Service Implements</typeparam>
        /// <param name="serviceEndpointUrl">Optional URL for the service Endpoint. Will use an application setting of "SERVICE_URL:Full.Namespace.For.T" by default.</param>
        /// <returns>An instance of the Interface T as a WCF Client.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "This is not Uri manager")]
        public static ChannelFactory<T> GetChannelFactory<T>(string serviceEndpointUrl = null)
        {
            // Endpoint is not specified in parameter - get it from App Settings
            if (string.IsNullOrWhiteSpace(serviceEndpointUrl))
            {
                // get default from config appsettings
                var configSetting = string.Format(CultureInfo.InvariantCulture, "UmaConnUrl:{0}", typeof(T).FullName);
                serviceEndpointUrl = ConfigurationManager.AppSettings[configSetting];
                if (string.IsNullOrWhiteSpace(serviceEndpointUrl))
                {
                    throw new ApplicationException(string.Format(CultureInfo.InvariantCulture, "Missing Application Setting for '{0}'", configSetting));
                }
            }

            var endpoint = new EndpointAddress(serviceEndpointUrl);
            if (!serviceEndpointUrl.StartsWith("net.tcp://", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ApplicationException(string.Format(CultureInfo.InvariantCulture, "UMA Connector Service endpoint must use NET.TCP, currently it is '{0}'", serviceEndpointUrl));
            }

            // raise binding reader quotas to sane limits
            var binding = new NetTcpBinding
                              {
                                  ReaderQuotas =
                                      {
                                          MaxStringContentLength = WcfSerializerMaxStringContentLength,
                                          MaxArrayLength = WcfSerializerMaxArrayLength,
                                          MaxBytesPerRead = WcfSerializerMaxBytesPerRead,
                                          MaxDepth = WcfSerializerMaxDepth,
                                          MaxNameTableCharCount = WcfSerializerMaxNameTableCharCount
                                      },
                                      CloseTimeout = new TimeSpan(0, 5, 0)
                              };
            var factory = new ChannelFactory<T>(binding, endpoint);
            factory.Endpoint.Behaviors.Add(new UmaConnHeaderBehavior());
            return factory;
        }
    }
}
