namespace Uma.Eservices.VetumaConn
{
    using System;

    /// <summary>
    /// IVetumaUtilities interface defines methods to get neccessary keys from config files
    /// </summary>
    public interface IVetumaUtilities
    {
        /// <summary>
        /// Returns key from config file as Uri
        /// </summary>
        /// <param name="input">VetumaKeys enum value</param>
        /// <returns>Uri object</returns>
        Uri GetConfigUriKey(VetumaKeys input);

        /// <summary>
        ///  Returns key from config file
        /// </summary>
        /// <param name="input">VetumaKeys enum value</param>
        /// <returns>Key value from config file</returns>
        string GetConfigKey(VetumaKeys input);
    }
}
