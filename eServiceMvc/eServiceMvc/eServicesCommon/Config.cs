namespace Uma.Eservices.Common
{
    using System.Configuration;

    /// <summary>
    /// Used for reading configuration from web.config
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Gets the RECaptcha public key.
        /// </summary>
        public static string RECaptchaPublicKey
        {
            get
            {
                return ConfigurationManager.AppSettings["RECaptchaPublicKey"];
            }
        }

        /// <summary>
        /// Gets the RECaptcha private key.
        /// </summary>
        public static string RECaptchaPrivateKey
        {
            get
            {
                return ConfigurationManager.AppSettings["RECaptchaPrivateKey"];
            }
        }
    }
}
