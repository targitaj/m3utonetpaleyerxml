namespace Uma.Eservices.VetumaConn
{
    /// <summary>
    /// Abstracts Vetuma payment interface in a mockable service
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Redirects the current user to the Vetuma payment interface
        /// </summary>
        /// <param name="paymentModel">Object model that contains necessary fields for payment process</param>
        void MakePayment(PaymentRequest paymentModel);

        /// <summary>
        /// Method that validates payment results. Do request to payment services use TransactionId as identifier for pay application
        /// </summary>
        /// <param name="transactionId">TransactionId unique payment identifier</param>
        /// <returns>PaymentResult object with payment response info</returns>
        PaymentResult ProcessResult(string transactionId);
    }
}
