namespace Uma.Eservices.Logic.Features.Account
{
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Models.Account;

    /// <summary>
    /// Manager for OWIN and MS Identity framework that does Login/Logoff functionalities together with all related to those
    /// </summary>
    public interface IApplicationSignInManager
    {
        /// <summary>
        /// Does actual LOGIN for user via username(e-mail) and password
        /// </summary>
        /// <param name="email">E-mail of a user</param>
        /// <param name="password">Password of a user</param>
        /// <param name="rememberMe">Should user login be persisted more for this login</param>
        /// <param name="shouldLockout">For locking accound if password is wrong. Locking is configured in ApplicationUserManager</param>
        Task<SignInStatus> PasswordSignInAsyncWrap(string email, string password, bool rememberMe, bool shouldLockout);

        /// <summary>
        /// Signs in Given application user
        /// </summary>
        /// <param name="user">Model for user</param>
        /// <param name="isPersistent">Determine if user is persistent</param>
        /// <param name="rememberBrowser">Determine if need to remember brouser</param>
        Task SignInAsyncWrap(WebUser user, bool isPersistent, bool rememberBrowser);

        /// <summary>
        /// Gets user Id from two-factor authentication cookie
        /// </summary>
        Task<int> GetVerifiedUserIdAsyncWrap();

        /// <summary>
        /// Sends the two factor code to caller.
        /// </summary>
        /// <param name="provider">The provider (SMS/e-mail).</param>
        /// <returns>Was sent success</returns>
        Task<bool> SendTwoFactorCodeAsyncWrap(string provider);

        /// <summary>
        /// Something around two-factor authenticated users
        /// </summary>
        Task<bool> HasBeenVerifiedAsyncWrap();

        /// <summary>
        /// Performs allowance for current user browser to access web application
        /// </summary>
        /// <param name="provider">SMS or e-mail</param>
        /// <param name="code">User entered verification code</param>
        /// <param name="isPersistent">Should it be persisted</param>
        /// <param name="rememberBrowser">Should browser be remembered to omit next time two factor code validation</param>
        Task<SignInStatus> TwoFactorSignInAsyncWrap(string provider, string code, bool isPersistent, bool rememberBrowser);

        /// <summary>
        /// Gets or sets the type of the authentication.
        /// </summary>
        string AuthenticationType { get; set; }

        /// <summary>
        /// Gets or sets the user manager.
        /// </summary>
        UserManager<ApplicationUser, int> UserManager { get; set; }

        /// <summary>
        /// Gets or sets the authentication manager.
        /// </summary>
        IAuthenticationManager AuthenticationManager { get; set; }

        /// <summary>
        /// Relogons the user asynchronous wrap.
        /// </summary>
        /// <param name="user">The user data.</param>
        /// <param name="isPersisted">If set to <c>true</c> [is persisted].</param>
        Task RelogonUserAsyncWrap(WebUser user, bool isPersisted);
    }
}