namespace Uma.Eservices.VetumaConn
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    /// <summary>
    /// Helper class for functionality used by both Vetuma authentication and payments
    /// </summary>
    public class VetumaUtilities : IVetumaUtilities
    {
        /// <summary>
        /// Returns key from config file as Uri
        /// </summary>
        /// <param name="input">VetumaKeys enum value</param>
        /// <returns>Uri object</returns>
        public Uri GetConfigUriKey(VetumaKeys input)
        {
            return new Uri(ConfigurationManager.AppSettings[input.ToString()]);
        }

        /// <summary>
        ///  Returns key from config file
        /// </summary>
        /// <param name="input">VetumaKeys enum value</param>
        /// <returns>Key value from config file</returns>
        public string GetConfigKey(VetumaKeys input)
        {
            return ConfigurationManager.AppSettings[input.ToString()];
        }
    }
}
