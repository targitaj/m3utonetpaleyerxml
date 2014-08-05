namespace Uma.Eservices.Logic.Features.Account
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Models.Account;

    /// <summary>
    /// Wrapper of UserManager of Identity Framework, containing its configuration and User management procedures
    /// </summary>
    public interface IApplicationUserManager
    {
        /// <summary>
        /// Generates the user identity asynchronously.
        /// </summary>
        /// <param name="user">The user object.</param>
        Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUser user);

        /// <summary>
        /// Generates the user identity asynchronously.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        Task<ClaimsIdentity> GenerateUserIdentityAsync(int userId);

        /// <summary>
        /// Creates the user in database, based on current known data (e-mail and password)
        /// </summary>
        /// <param name="eMail">The e mail of a user.</param>
        /// <param name="password">The password, user specified.</param>
        /// <returns>Tuple result, Item1 = <see cref="IdentityResult"/>, Item2 = Created User ID</returns>
        Task<Tuple<IdentityResult, int>> CreateAsyncWrap(string eMail, string password);

        /// <summary>
        /// Retrieve list of defined providers (SMS/e-mail) for sending security code
        /// </summary>
        /// <param name="userId">ID of a user in question</param>
        /// <returns>List of verification code send options (SMS/Email)</returns>
        Task<IList<string>> GetValidTwoFactorProvidersAsyncWrap(int userId);

        /// <summary>
        /// Retrieves Token of E-mail confirmation to send it via e-mail to get user verification of registration
        /// </summary>
        /// <param name="userId">ID of a user in question</param>
        /// <returns>Verification code (encrypted)</returns>
        Task<string> GenerateEmailConfirmationTokenAsyncWrap(int userId);

        /// <summary>
        /// Wrapper for e-mail send service for OWIN / Identity use.
        /// </summary>
        /// <param name="userId">ID of a user</param>
        /// <param name="subject">Subject o a e-mail</param>
        /// <param name="body">Body (message itself)</param>
        Task SendEmailAsyncWrap(int userId, string subject, string body);

        /// <summary>
        /// Returns UI model fro user counterpart in security persistence
        /// </summary>
        /// <param name="userId">ID of persisted user</param>
        Task<WebUser> FindByIdAsyncWrap(int userId);

        /// <summary>
        /// Returns Toke (Code) for two-factor authentication (Browser access allowance)
        /// </summary>
        /// <param name="userId">ID of a user</param>
        /// <param name="sendProviderName">Provider name (SMSM or e-mail)</param>
        Task<string> GenerateTwoFactorTokenAsyncWrap(int userId, string sendProviderName);

        /// <summary>
        /// E-mail confirmation method wrapper
        /// </summary>
        /// <param name="userId">ID of a user</param>
        /// <param name="confirmationCode">Confirmation code</param>
        Task<IdentityResult> ConfirmEmailAsyncWrap(int userId, string confirmationCode);

        /// <summary>
        /// Returns User data by its UserName (email)
        /// </summary>
        /// <param name="emailAddress">Email address (which is username)</param>
        Task<WebUser> FindByNameAsyncWrap(string emailAddress);

        /// <summary>
        /// Returns User data by its UserName (email)
        /// </summary>
        /// <param name="emailAddress">Email address (which is username)</param>
        Task<WebUser> FindByEmailAsyncWrap(string emailAddress);

        /// <summary>
        /// Determines whether is email confirmed for the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        Task<bool> IsEmailConfirmedAsyncWrap(int userId);

        /// <summary>
        /// Switch for Two-Factor authentication for user profile.
        /// Should be used for weak authenticated users, turn off for strong authentication users
        /// </summary>
        /// <param name="userId">Identifier of a user</param>
        /// <param name="secondFactorSwitch">If true - will turn on, false - turn off</param>
        Task<IdentityResult> SetTwoFactorEnabledAsyncWrap(int userId, bool secondFactorSwitch);

        /// <summary>
        /// Wrapper around create Reset password token for specific user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Reset password token as string</returns>
        Task<string> GeneratePasswordResetTokenAsyncWrap(int userId);

        /// <summary>
        /// Resets the user password, if token is correct for him
        /// </summary>
        /// <param name="userId">User unique identifier</param>
        /// <param name="resetToken">Token that has been sent to user in e-mail link</param>
        /// <param name="newPassword">New password which user entered in Reset password screen</param>
        Task<IdentityResult> ResetPasswordAsyncWrap(int userId, string resetToken, string newPassword);

        /// <summary>
        /// Changes the user's password asynchronously
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        Task<IdentityResult> ChangePasswordAsyncWrap(int userId, string oldPassword, string newPassword);

        /// <summary>
        /// Updates the application user asynchronously.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userData">The user data from profile.</param>
        Task<IdentityResult> UpdateAsyncWrap(int userId, WebUser userData);

        /// <summary>
        /// Resets User e-mail address to other, resetting Verification token
        /// </summary>
        /// <param name="userId">Unique identifier of a user</param>
        /// <param name="emailAddress">New e-mail address</param>
        /// <returns>Result of operation</returns>
        Task<IdentityResult> SetEmailAsyncWrap(int userId, string emailAddress);

        /// <summary>
        /// Retrieves WebUser data from database of users by his PersonCode (HETU)
        /// </summary>
        /// <param name="personCode">Finnish PersonCode (HETU)</param>
        /// <returns>WebUser or NULL if not found</returns>
        WebUser FindByPersonCodeStrong(string personCode);
    }
}