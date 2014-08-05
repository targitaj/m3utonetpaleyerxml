namespace Uma.Eservices.Logic.Features.VetumaService
{
    using System;
    using Uma.Eservices.Models.Account;
    using Uma.Eservices.Models.Vetuma;

    /// <summary>
    /// Interface specifies methods for Vetuma authorization implementation
    /// </summary>
    public interface IVetumaAuthenticationLogic
    {
        /// <summary>
        /// Authenticate creates, fill required fields for Vetuma auth action
        /// </summary>
        /// <param name="trancationId">Unique transaction Id</param>
        /// <param name="uriModel">Uri model that contains uris for Error/cancel/sucess case</param>
        void Authenticate(string trancationId, VetumaUriModel uriModel);

        /// <summary>
        /// Validates auth response and response results
        /// </summary>
        /// <param name="transactionId">Unique transaction Id</param>
        /// <returns>Application user</returns>
        WebUser ProcessAuthenticationResult(string transactionId);
    }
}