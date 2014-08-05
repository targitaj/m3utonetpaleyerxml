namespace Uma.Eservices.VetumaConn
{
    using Fujitsu.Vetuma.Toolkit;

    /// <summary>
    /// IVetumaService  interface that describes Vetuma service extrnal method calls
    /// used for uni testing generally
    /// </summary>
    public interface IVetumaService
    {
        /// <summary>
        /// SubmitPaymentRequest used to submit Vetuma payment
        /// </summary>
        /// <param name="request">VetumaPaymentRequest model</param>
        void SubmitPaymentRequest(VetumaPaymentRequest request);

        /// <summary>
        /// PaymentResponseValidate used to validate payment
        /// </summary>
        /// <param name="model">VetumaPaymentResponse model</param>
        /// <returns>True if payment was valid, False if not</returns>
        bool PaymentResponseValidate(VetumaPaymentResponse model);

        /// <summary>
        /// Creates VetumaPaymentResponse for testing and for real call
        /// </summary>
        /// <param name="sharedSecretId">SharedSecretId value</param>
        /// <param name="sharedSecret">SharedSecret value</param>
        /// <returns>VetumaPaymentResponse object</returns>
        VetumaPaymentResponse CreateVetumaPaymentResponse(string sharedSecretId, string sharedSecret);

        /// <summary>
        /// Method submit Vetuma authentification request
        /// </summary>
        /// <param name="input">VetumaAuthenticationRequest object</param>
        void SubmitVetumaAuthenticationRequest(VetumaAuthenticationRequest input);

        /// <summary>
        /// Creates VetumaAuthenticationResponse for testing and for real call
        /// </summary>
        /// <param name="sharedSecretId">SharedSecretId value</param>
        /// <param name="sharedSecret">SharedSecret value</param>
        /// <returns>VetumaAuthenticationResponse object</returns>
        VetumaAuthenticationResponse CreateVetumaAuthentificationResponse(string sharedSecretId, string sharedSecret);
    }
}
