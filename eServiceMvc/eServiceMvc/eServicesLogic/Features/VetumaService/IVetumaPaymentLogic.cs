namespace Uma.Eservices.Logic.Features.VetumaService
{
    using System.Collections.Generic;
    using Uma.Eservices.Models.Vetuma;

    /// <summary>
    /// Vetuma payment process implementation interface
    /// </summary>
    public interface IVetumaPaymentLogic
    {
        /// <summary>
        /// Method validates input params and redirects to Vetuma payment service
        /// </summary>
        /// <param name="model">VetumaPaymentModel object</param>
        void MakePayment(VetumaPaymentModel model);

        /// <summary>
        /// Validates payment response and respone fields
        /// </summary>
        /// <param name="applicationid">Unique applicationid ID</param>
        /// <returns>PaymentModel object</returns>
        PaymentModel ProcessResult(int applicationid);

        /// <summary>
        /// Checks if application is already paid.
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <returns>True if paid, false if not</returns>
        bool IsApplicationPaid(int applicationId);
    }
}