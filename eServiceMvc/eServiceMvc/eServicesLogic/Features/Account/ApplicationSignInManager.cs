namespace Uma.Eservices.Logic.Features.Account
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Models.Account;

    /// <summary>
    /// Manager for OWIN and MS Identity framework that does Login/Logoff functionalities together with all related to those
    /// </summary>
    public class ApplicationSignInManager : SignInManager<ApplicationUser, int>, IApplicationSignInManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSignInManager"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="authenticationManager">The authentication manager.</param>
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager) { }

        /// <summary>
        /// Method moved from User object (as in sample) to avoid layer cross-referencing
        /// </summary>
        /// <param name="user">User person data</param>
        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            var userIdentity = this.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        /// <summary>
        /// Static creator for SignInManager. Used by OWIN config to create it when necessary
        /// </summary>
        /// <param name="options">OWIN passed options</param>
        /// <param name="context">OWIN context</param>
        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

        /// <summary>
        /// Does actual LOGIN for user via username(e-mail) and password
        /// </summary>
        /// <param name="email">E-mail of a user</param>
        /// <param name="password">Password of a user</param>
        /// <param name="rememberMe">Should user login be persisted more for this login</param>
        /// <param name="shouldLockout">For locking accound if password is wrong. Locking is configured in ApplicationUserManager</param>
        public Task<SignInStatus> PasswordSignInAsyncWrap(string email, string password, bool rememberMe, bool shouldLockout)
        {
            return this.PasswordSignInAsync(email, password, rememberMe, shouldLockout);
        }

        /// <summary>
        /// Signs in Given application user
        /// </summary>
        /// <param name="user">Model for user</param>
        /// <param name="isPersistent">Determine if user is persistent</param>
        /// <param name="rememberBrowser">Determine if need to remember brouser</param>
        public async Task SignInAsyncWrap(WebUser user, bool isPersistent, bool rememberBrowser)
        {
            if (user.Id == 0)
            {
                throw new ArgumentException("User to SignIn is missing ID", "user");
            }

            ApplicationUser appUser = await this.UserManager.FindByIdAsync(user.Id);
            await this.SignInAsync(appUser, isPersistent, rememberBrowser);
        }

        /// <summary>
        /// Gets user Id from two-factor authentication cookie
        /// </summary>
        public Task<int> GetVerifiedUserIdAsyncWrap()
        {
            // Uses Two-Factor authentication cookie to retrieve user id
            return this.GetVerifiedUserIdAsync();
        }

        /// <summary>
        /// Sends the two factor code to caller.
        /// </summary>
        /// <param name="provider">The provider (SMS/e-mail).</param>
        /// <returns>Was sent success</returns>
        public Task<bool> SendTwoFactorCodeAsyncWrap(string provider)
        {
            return this.SendTwoFactorCodeAsync(provider);
        }

        /// <summary>
        /// Something around two-factor authenticated users
        /// </summary>
        public async Task<bool> HasBeenVerifiedAsyncWrap()
        {
            return await this.HasBeenVerifiedAsync();
        }

        /// <summary>
        /// Performs allowance for current user browser to access web application
        /// </summary>
        /// <param name="provider">SMS or e-mail</param>
        /// <param name="code">User entered verification code</param>
        /// <param name="isPersistent">Should it be persisted</param>
        /// <param name="rememberBrowser">Should browser be remembered to omit next time two factor code validation</param>
        public Task<SignInStatus> TwoFactorSignInAsyncWrap(string provider, string code, bool isPersistent, bool rememberBrowser)
        {
            return this.TwoFactorSignInAsync(provider, code, isPersistent, rememberBrowser);
        }

        /// <summary>
        /// Performs relogging of user (if its properties changed)
        /// </summary>
        /// <param name="user">UI User object</param>
        /// <param name="isPersistent">Should it be persisted for browser</param>
        public async Task RelogonUserAsyncWrap(WebUser user, bool isPersistent)
        {
            this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            this.AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await ((ApplicationUserManager)this.UserManager).GenerateUserIdentityAsync(user.Id));
        }
    }
}
