namespace Uma.Eservices.VetumaConn
{
    using System.Diagnostics.CodeAnalysis;
    using Fujitsu.Vetuma.Toolkit;

    [ExcludeFromCodeCoverage]
    /// <summary>
    /// IVetumaService interface implementations
    /// Used only for request submittion and response validation
    /// </summary>
    public class VetumaServiceSubmit : IVetumaService
    {
        /// <summary>
        /// SubmitPaymentRequest used to submit Vetuma payment
        /// </summary>
        /// <param name="request">VetumaPaymentRequest model</param>
        public void SubmitPaymentRequest(Fujitsu.Vetuma.Toolkit.VetumaPaymentRequest request)
        {
            request.Submit();
        }

        /// <summary>
        /// PaymentResponseValidate used to validate payment
        /// </summary>
        /// <param name="model">VetumaPaymentResponse model</param>
        /// <returns>True if payment was valid, False if not</returns>
        public bool PaymentResponseValidate(Fujitsu.Vetuma.Toolkit.VetumaPaymentResponse model)
        {
            return model.Validate();
        }

        /// <summary>
        /// Creates VetumaPaymentResponse for testing and for real call
        /// </summary>
        /// <param name="sharedSecretId">SharedSecretId value</param>
        /// <param name="sharedSecret">SharedSecret value</param>
        /// <returns>VetumaPaymentResponse object</returns>
        public VetumaPaymentResponse CreateVetumaPaymentResponse(string sharedSecretId, string sharedSecret)
        {
            return new VetumaPaymentResponse(sharedSecretId, sharedSecret);
        }

        /// <summary>
        /// Method submit Vetuma authentification request
        /// </summary>
        /// <param name="input">VetumaAuthenticationRequest object</param>
        public void SubmitVetumaAuthenticationRequest(VetumaAuthenticationRequest input)
        {
            input.Submit();
        }

        /// <summary>
        /// Creates VetumaAuthenticationResponse for testing and for real call
        /// </summary>
        /// <param name="sharedSecretId">SharedSecretId value</param>
        /// <param name="sharedSecret">SharedSecret value</param>
        /// <returns>VetumaAuthenticationResponse object</returns>
        public VetumaAuthenticationResponse CreateVetumaAuthentificationResponse(string sharedSecretId, string sharedSecret)
        {
            return new VetumaAuthenticationResponse(sharedSecretId, sharedSecret);
        }
    }
}
