namespace Uma.Eservices.Web.Features.Account
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Uma.Eservices.Logic.Features.Account;
    using Uma.Eservices.Logic.Features.VetumaService;
    using Uma.Eservices.Models.Account;
    using Uma.Eservices.Web.Core;

    /// <summary>
    /// Everything related to VetumaServiceController management
    /// </summary>
    public partial class StrongAuthenticationController : BaseController
    {
        /// <summary>
        /// Identifier for the Vetuma request-specific unique id stored in the session
        /// </summary>
        private const string KeyNameTransactionIdentifierKey = "VETUMA_TRANSACTION_ID";

        /// <summary>
        /// IVetumaAuthenticationLogic instance property
        /// </summary>
        private IVetumaAuthenticationLogic VetumaAuth { get; set; }

        /// <summary>
        /// Identity Usermanager wrapper holder
        /// </summary>
        private IApplicationUserManager userManager;

        /// <summary>
        /// MS Identity and OWIN injected user Manager
        /// </summary>
        public IApplicationUserManager UserManager
        {
            get
            {
                return this.userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                this.userManager = value;
            }
        }

        /// <summary>
        /// Holder for SingIn manager of MS identity / OWIN
        /// </summary>
        private IApplicationSignInManager signInManager;

        /// <summary>
        /// MS Identity and OWIN required SingIn manager
        /// </summary>
        public IApplicationSignInManager SignInManager
        {
            get
            {
                return this.signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }

            private set
            {
                this.signInManager = value;
            }
        }

        /// <summary>
        /// Overloaded controller constuctor user to get instances of: IAuthenticationManager, IVetumaAuthenticationLogic
        /// </summary>
        /// <param name="vetumaLogic">IVetumaAuthenticationLogic instance</param>
        public StrongAuthenticationController(IVetumaAuthenticationLogic vetumaLogic)
        {
            this.VetumaAuth = vetumaLogic;
        }

        /// <summary>
        /// Action  Authenticates via Vetuma service. Returns empty action
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public virtual EmptyResult Authenticate()
        {
            Uri errorCancelUri = new Uri(@"https://" + this.Request.Url.Authority + Url.Action(MVC.StrongAuthentication.ActionNames.VetumaCancel, MVC.StrongAuthentication.Name), UriKind.Absolute);
            Uri redirectUri = new Uri(@"https://" + this.Request.Url.Authority + Url.Action(MVC.StrongAuthentication.ActionNames.ProcessResult, MVC.StrongAuthentication.Name), UriKind.Absolute);
            this.VetumaAuth.Authenticate(this.GenerateAndStoreUniqueId(KeyNameTransactionIdentifierKey), new Models.Vetuma.VetumaUriModel
            {
                CancelUri = errorCancelUri,
                ErrorUri = errorCancelUri,
                RedirectUri = redirectUri
            });
            return new EmptyResult();
        }

        /// <summary>
        /// Return path for Vetuma Errors or Cancelling out of Vetuma
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public virtual ActionResult VetumaCancel()
        {
            // TODO: Create object of CANCEL/ERROR return from Vetuma and show response reason more detailed
            return this.View(MVC.Account.Views.VetumaCancel);
        }

        /// <summary>
        /// Vetuma authentification process redirects to this action. Method validates Vetuma response
        /// If all OK with Vetuma, checks whether user exists in eService (Yes=Login, No=Create through Profile edit page)
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public virtual async Task<ActionResult> ProcessResult()
        {
            WebUser strongUser = this.VetumaAuth.ProcessAuthenticationResult(this.GetAndClearUniqueId(KeyNameTransactionIdentifierKey));

            // Existing Login - upgrade to strong operation
            if (User.Identity.IsAuthenticated)
            {
                WebUser weakUser = await this.UserManager.FindByIdAsyncWrap(int.Parse(User.Identity.GetUserId()));
                weakUser.FirstName = strongUser.FirstName;
                weakUser.LastName = strongUser.LastName;
                weakUser.PersonCode = strongUser.PersonCode;
                weakUser.BirthDate = strongUser.BirthDate;
                weakUser.IsStronglyAuthenticated = strongUser.IsStronglyAuthenticated;
                await this.UserManager.UpdateAsyncWrap(weakUser.Id, weakUser);
                await this.UserManager.SetTwoFactorEnabledAsyncWrap(weakUser.Id, false);
                await this.SignInManager.RelogonUserAsyncWrap(weakUser, false);
                return this.RedirectToAction(MVC.Account.UserProfile());
            }

            WebUser webUser = this.UserManager.FindByPersonCodeStrong(strongUser.PersonCode) ?? new WebUser();
            webUser.FirstName = strongUser.FirstName;
            webUser.LastName = strongUser.LastName;
            webUser.PersonCode = strongUser.PersonCode;
            webUser.BirthDate = strongUser.BirthDate;
            webUser.IsStronglyAuthenticated = strongUser.IsStronglyAuthenticated;
            if (!string.IsNullOrEmpty(webUser.UserName) && webUser.Id != 0)
            {
                // Existing user - just login him
                await this.SignInManager.SignInAsyncWrap(webUser, true, false);
                await this.UserManager.UpdateAsyncWrap(webUser.Id, webUser);

                // TODO: Check whether CustomerID is null and lookup it in UMA to link them.
                return this.RedirectToAction(MVC.Dashboard.Index());
            }

            // New user from Vetuma - redirect to account additional information page
            return this.View(MVC.Account.Views.UserProfileStrongEdit, webUser.ToUserProfileModel());
        }

        /// <summary>
        ///  See: http://brockallen.com/2013/10/24/a-primer-on-owin-cookie-authentication-middleware-for-the-asp-net-developer/
        ///  SignIn user via Owin
        ///  ClaimTypes.NameIdentifier is unique claim type
        /// </summary>
        /// <param name="user">The user object, validated.</param>
        private void SignInVetumaUser(WebUser user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));

            var id = new ClaimsIdentity(claims,
                                        DefaultAuthenticationTypes.ApplicationCookie);

            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignIn(id);
        }

        /// <summary>
        /// Create a unique identifier (of type string, 20 char long) and add it to the current user's session
        /// </summary>
        /// <param name="keyName">Key name, Session dictionary identifier</param>
        /// <returns>The identifier that was stored</returns>
        private string GenerateAndStoreUniqueId(string keyName)
        {
            // Vetuma only allows a 20 char alphanumeric transactionID, so can't use a complete guid
            string identifier = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 20);
            this.HttpContext.Session.Add(keyName, identifier);
            return identifier;
        }

        /// <summary>
        /// Compares the unique id to the one stored in the user's session. Clears the uniqueId from the session.
        /// </summary>
        /// <param name="keyName">Identifier returned from Vetuma</param>
        /// <returns>True if the identifiers match each other</returns>
        /// <exception cref="System.InvalidOperationException">Throws exception if the unique id isn't found (no corresponding request?)</exception>
        private string GetAndClearUniqueId(string keyName)
        {
            if (this.Session[keyName] == null)
            {
                throw new InvalidOperationException(keyName + "Does not exist in current session context");
            }

            string sessionGuid = this.HttpContext.Session[keyName].ToString();
            this.HttpContext.Session.Remove(keyName);
            return sessionGuid;
        }
    }
}