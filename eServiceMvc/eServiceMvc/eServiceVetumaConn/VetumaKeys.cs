namespace Uma.Eservices.VetumaConn
{
    /// <summary>
    /// Vetuma application config key enum
    /// </summary>
    public enum VetumaKeys
    {
        /// <summary>
        /// Key for the Vetuma authentication service url in the config file
        /// </summary>
        VetumaAuthenticationUrl,

        /// <summary>
        /// Key for the Vetuma payment service url in the config file
        /// </summary>
        VetumaPaymentUrl,

        /// <summary>
        /// Key for the Vetuma authentication SharedSecretId in the config file
        /// </summary>
        VetumaAuthenticationSharedSecretId,

        /// <summary>
        /// Key for the Vetuma police authentication SharedSecretId in the config file
        /// </summary>
        VetumaPoliceAuthenticationSharedSecretId,

        /// <summary>
        /// Key for the Vetuma SharedSecretId in the config file
        /// </summary>
        VetumaPaymentSharedSecretId,

        /// <summary>
        /// Key for the Vetuma police SharedSecretId in the config file
        /// </summary>
        VetumaPolicePaymentSharedSecretId,

        /// <summary>
        /// Key for the Vetuma authentication SharedSecret in the config file
        /// </summary>
        VetumaAuthenticationSharedSecret,

        /// <summary>
        /// Key for the Vetuma police authentication SharedSecret in the config file
        /// </summary>
        VetumaPoliceAuthenticationSharedSecret,

        /// <summary>
        /// Key for the Vetuma payment SharedSecret in the config file
        /// </summary>
        VetumaPaymentSharedSecret,

        /// <summary>
        /// Key for the Vetuma police payment SharedSecret in the config file
        /// </summary>
        VetumaPolicePaymentSharedSecret,

        /// <summary>
        /// Key for the Vetuma authentication ConfigurationId in the config file
        /// </summary>
        VetumaAuthenticationConfigurationId,

        /// <summary>
        /// Key for the Vetuma police authentication ConfigurationId in the config file
        /// </summary>
        VetumaPoliceAuthenticationConfigurationId,

        /// <summary>
        /// Key for the Vetuma authentication ConfigurationId in the config file
        /// </summary>
        VetumaPaymentConfigurationId,

        /// <summary>
        /// Key for the Vetuma police authentication ConfigurationId in the config file
        /// </summary>
        VetumaPolicePaymentConfigurationId,

        /// <summary>
        /// Key for the Vetuma application identifier in the config file
        /// </summary>
        VetumaApplicationIdentifier,

        /// <summary>
        /// Key for the Vetuma police application identifier in the config file
        /// </summary>
        VetumaPoliceApplicationIdentifier,

        /// <summary>
        /// Key for the Vetuma application display name in the config file
        /// </summary>
        VetumaApplicationDisplayName,

        /// <summary>
        /// String returned by Vetuma to indicate successful authentication
        /// </summary>
        SUCCESSFUL,

        /// <summary>
        /// Identifier for the Vetuma request-specific unique id stored in the session
        /// </summary>
        VETUMA_TRANSACTION_ID
    }
}
