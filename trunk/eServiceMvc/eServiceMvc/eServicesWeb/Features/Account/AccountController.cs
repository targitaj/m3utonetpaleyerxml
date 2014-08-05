namespace Uma.Eservices.Web.Features.Account
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI.WebControls;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using Uma.Eservices.Logic.Features.Account;
    using Uma.Eservices.Models.Account;
    using Uma.Eservices.Web.Components;
    using Uma.Eservices.Web.Core;
    using Uma.Eservices.Web.Core.Filters;

    /// <summary>
    /// Everything related to user account management
    /// </summary>
    public partial class AccountController : BaseController
    {
        /// <summary>
        /// Used for XSRF protection when adding external logins
        /// </summary>
        private const string XsrfKey = "XsrfId";

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

            set
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

            set
            {
                this.signInManager = value;
            }
        }

        /// <summary>
        /// Authentication manager
        /// </summary>
        private IAuthenticationManager authenticationManager;

        /// <summary>
        /// Authentication manager for user authentication logic functions
        /// </summary>
        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                return this.authenticationManager ?? HttpContext.GetOwinContext().Authentication;
            }

            set
            {
                this.authenticationManager = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController" /> class.
        /// </summary>
        public AccountController()
        {
        }

        /// <summary>
        /// Prepares and launches login screen
        /// </summary>
        /// <param name="returnUrl">Used to have ability return to necessary page</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "Would require owerwrite MVC internals")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Justification = "Oh, come ON!")]
        [AllowAnonymous]
        [HttpGet]
        public virtual ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                this.WebMessages.AddInfoMessage(this.T("SignIn"), this.T("You are already signed in"));
                return this.RedirectToAction(MVC.Dashboard.Index());
            }

            return this.View(MVC.Account.Views.LoginForm, new LoginViewModel { ReturnUrl = returnUrl });
        }

        /// <summary>
        /// Entry point where used login data is received.
        /// </summary>
        /// <param name="model">The model with email password and other fields.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Justification = "Why the hell Login is a bad word?")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [DatabaseTransaction]
        public virtual async Task<ActionResult> Login(LoginViewModel model)
        {
            if (model == null)
            {
                return this.View(MVC.Account.Views.LoginForm, new LoginViewModel());
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(MVC.Account.Views.LoginForm, model);
            }

            var user = await this.UserManager.FindByNameAsyncWrap(model.Email);
            if (user != null)
            {
                // Upgraded users should not use weak authentication anymore
                if (user.IsStronglyAuthenticated)
                {
                    this.WebMessages.AddInfoMessage(this.T("SignIn"), this.T("This user has upgraded account to strong authentication. Please use Login through BankID, ID card or Mobile certificate option."));
                    return this.RedirectToAction(MVC.Account.Login());
                }

                // Require the user to have a confirmed email before they can log on.
                if (!await this.UserManager.IsEmailConfirmedAsyncWrap(user.Id))
                {
                    return this.View(MVC.Account.Views.ViewNames.EmailNotConfirmed, model: model.Email);
                }
            }

            SignInStatus result = await this.SignInManager.PasswordSignInAsyncWrap(model.Email, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    WebUser userData = await this.UserManager.FindByEmailAsyncWrap(model.Email);
                    if (string.IsNullOrEmpty(userData.FirstName) || string.IsNullOrEmpty(userData.LastName) || userData.BirthDate == null)
                    {
                        return this.RedirectToAction(MVC.Account.ActionNames.UserProfileFullEdit, MVC.Account.Name);
                    }

                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return this.Redirect(model.ReturnUrl);
                    }

                    return this.RedirectToAction(MVC.Dashboard.ActionNames.Index, MVC.Dashboard.Name);
                case SignInStatus.LockedOut:
                    return this.View(MVC.Account.Views.Lockout);
                case SignInStatus.RequiresVerification:
                    return this.RedirectToAction(MVC.Account.SendCodeChoice(model.ReturnUrl));
                case SignInStatus.Failure:
                default:
                    // TODO: Ensure these strigs are added to translations
                    string title = this.T("Authentication error");
                    this.WebMessages.AddErrorMessage(this.T("Authentication error"), this.T("Invalid email or password."));
                    return this.View(MVC.Account.Views.ViewNames.LoginForm, model);
            }
        }

        /// <summary>
        /// Provides screen for choosing how to sent two-factor authentication.
        /// This is for sending numerical code to user via  SMS or e-mail to authenticate current used browser
        /// </summary>
        /// <param name="returnUrl">The return URL (before login).</param>
        /// <returns>Either choose screen or sending code to default and redirecting to code input page</returns>
        [AllowAnonymous]
        public virtual async Task<ActionResult> SendCodeChoice(string returnUrl)
        {
            // from two-factor authentication cookie
            int userId = await this.SignInManager.GetVerifiedUserIdAsyncWrap();

            // What two-factor authentications are available, set in Uma.Eservices.Logic.Features.Account.ApplicationUserManager
            var userFactors = await this.UserManager.GetValidTwoFactorProvidersAsyncWrap(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            if (factorOptions.Count == 1)
            {
                // Omit chooser when there is only one option
                return await this.SendCode(new SendCodeViewModel { SelectedProvider = userFactors[0], ReturnUrl = returnUrl });
            }

            return this.View(MVC.Account.Views.SendCodeChoice, new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl });
        }

        /// <summary>
        /// Called either from second factor method <see cref="SendCodeChoice(string)"/> 
        /// </summary>
        /// <param name="model">Model containing choice of through which provider to send code (SMS or e-mail)</param>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(MVC.Account.Views.SendCodeChoice);
            }

            // Generate the token and send it
            if (!await this.SignInManager.SendTwoFactorCodeAsyncWrap(model.SelectedProvider))
            {
                // TODO: Add this to translations master data
                this.WebMessages.AddErrorMessage(this.T("Application error"), this.T("There were problems sending verification code"));
                return this.RedirectToAction(MVC.Dashboard.ActionNames.Index, MVC.Dashboard.Name);
            }

            return this.RedirectToAction(MVC.Account.VerifyCode(provider: model.SelectedProvider, returnUrl: model.ReturnUrl));
        }

        /// <summary>
        /// This method is used to enter browser access verification code (two-factor) which normally is 
        /// sent to user via SMSm or e-mail.
        /// </summary>
        /// <param name="provider">Which provider is used (SMSM or e-mail), set in <see cref="Uma.Eservices.Logic.Features.Account.ApplicationUserManager"/></param>
        /// <param name="returnUrl">Return URL, which is dragged over to return user to previous page</param>
        [AllowAnonymous]
        public virtual async Task<ActionResult> VerifyCode(string provider, string returnUrl)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await this.SignInManager.HasBeenVerifiedAsyncWrap())
            {
                this.WebMessages.AddErrorMessage(this.T("Application error"), this.T("There were problems verifying code"));
                return this.RedirectToAction(MVC.Dashboard.ActionNames.Index, MVC.Dashboard.Name);
            }
#if !PROD
            // Retrieves user object data via two-factor authentication cookie
            var user = await this.UserManager.FindByIdAsyncWrap(await this.SignInManager.GetVerifiedUserIdAsyncWrap());
            if (user != null)
            {
                ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " + await this.UserManager.GenerateTwoFactorTokenAsyncWrap(user.Id, provider);
            }
#endif
            return this.View(MVC.Account.Views.VerifyCode, new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl });
        }

        /// <summary>
        /// Verification code entry POST action to accept user entered verification code
        /// </summary>
        /// <param name="model">Model which contains entered code</param>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(MVC.Account.Views.VerifyCode, model);
            }

            var result = await this.SignInManager.TwoFactorSignInAsyncWrap(model.Provider, model.Code, isPersistent: false, rememberBrowser: true); //model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return this.Redirect(model.ReturnUrl);
                    }

                    // Goes to dashboard or Login, if user is not logged in yet
                    return this.RedirectToAction(MVC.Dashboard.ActionNames.Index, MVC.Dashboard.Name);
                case SignInStatus.LockedOut:
                    return this.View(MVC.Account.Views.Lockout);
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError(string.Empty, "Invalid code");
                    return this.View(MVC.Account.Views.VerifyCode, model);
            }
        }

        /// <summary>
        /// Prepares and launches user registration form
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public virtual ActionResult Register()
        {
            return this.View(MVC.Account.Views.RegistrationForm, new RegistrationViewModel());
        }
        
        /// <summary>
        /// Receives submitted registration form contents and perorms actions based on entries in form.
        /// </summary>
        /// <param name="model">The model of registration form.</param>
        [AllowAnonymous]
        [HttpPost]
        [DatabaseTransaction]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Register(RegistrationViewModel model)
        {
            if (model == null)
            {
                return this.View(MVC.Account.Views.RegistrationForm, new RegistrationViewModel());
            }

            ExtensionMethodExecuter.IsCaptchaValid(this, T("Captcha is not valid."));

            if (!this.ModelState.IsValid)
            {
                return this.View(MVC.Account.Views.RegistrationForm, model);
            }

            // Create user object in database
            var result = await this.UserManager.CreateAsyncWrap(model.Email, model.Password);
            if (result.Item1.Succeeded && result.Item2 != 0)
            {
                int userId = result.Item2;

                // Create all necessary information to send e-mail verification
                string callbackUrl = await this.SendEmailVerificationToken(userId);
#if !PROD
                // For testing this will show same link in next page, to ease confirmation procedure
                ViewBag.Link = callbackUrl;
#endif
                return this.View(MVC.Account.Views.ViewNames.DisplayEmailConfirmation);
            }

            foreach (var errorMessage in result.Item1.Errors)
            {
                this.WebMessages.AddErrorMessage(errorMessage);
            }

            return this.View(MVC.Account.Views.RegistrationForm, model);
        }

        /// <summary>
        /// Sends the vemail verification token to user and returns Callback URL (for testing purposes)
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        protected virtual async Task<string> SendEmailVerificationToken(int userId)
        {
            string code = await this.UserManager.GenerateEmailConfirmationTokenAsyncWrap(userId);
            var callbackUrl = this.Url.Action(MVC.Account.ActionNames.ConfirmEmail, MVC.Account.Name, new { userId = userId, code = code }, protocol: this.Request.Url.Scheme);

            // e-mail service is configured as Interface implementation and assigned in ApplicationUserManager
            // TODO: Add these strings to translation master data
            string subject = this.T("Confirm your account with Finnish Immigration eServices");
            string body =
                this.T("You have started the registration for Immigration eServices. You should continue by selecting this <a href='{0}'>link</a>. By selecting the link you can finalize your registration information and activate your Immigration eService account.", callbackUrl);
            await this.UserManager.SendEmailAsyncWrap(userId, subject, body);
            return callbackUrl;
        }

        /// <summary>
        /// This method should receive click action from confirmation link sent in e-mail to user after registration
        /// </summary>
        /// <param name="userId">ID of a user</param>
        /// <param name="code">Verification code itself</param>
        [AllowAnonymous]
        public virtual async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                // TODO: Add tranalations to master data
                this.WebMessages.AddErrorMessage(this.T("Confirmation error"), this.T("Confirmation did not contain required data."));
                return this.RedirectBack();
            }

            int userIdentifier = 0;
            if (!int.TryParse(userId, out userIdentifier) || userIdentifier == 0)
            {
                // TODO: Add tranalations to master data
                this.WebMessages.AddErrorMessage(this.T("Confirmation error"), this.T("Confirmation did not contain required data."));
                return this.RedirectBack();
            }

            WebUser user = await this.UserManager.FindByIdAsyncWrap(userIdentifier);
            if (user.EmailConfirmed)
            {
                // TODO: Add tranalations to master data
                this.WebMessages.AddErrorMessage(this.T("Confirmation error"), this.T("This account already has been registered."));
                return this.RedirectBack();
            }

            // Calls method to set flag for user that e-mail is confirmed
            IdentityResult result = await this.UserManager.ConfirmEmailAsyncWrap(userIdentifier, code);
            if (result.Succeeded)
            {
                // Check whether user has Strong Authentication - then do not enable 2nd auth step
                if (!user.IsStronglyAuthenticated)
                {
                    await this.UserManager.SetTwoFactorEnabledAsyncWrap(int.Parse(userId), true);
                }

                return this.View(MVC.Account.Views.EmailVerified);
            }

            // TODO: Add translation to master data, when available
            this.WebMessages.AddErrorMessage(this.T("Confirmation error"), this.T("Either confirmation code expired or it is wrong."));
            return this.RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
        }

        /// <summary>
        /// Resends e-mail verification token to user
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        [AllowAnonymous]
        public virtual async Task<ActionResult> ConfirmationEmailResend(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
            {
                // TODO: Add to master data translations!
                this.WebMessages.AddErrorMessage(this.T("Data error"), this.T("Email verification missing person e-mail address."));
                return this.RedirectBack();
            }

            WebUser user = await this.UserManager.FindByEmailAsyncWrap(emailAddress);
            if (user == null)
            {
                // TODO: Add to master data translations!
                this.WebMessages.AddErrorMessage(this.T("Data error"), this.T("There was an error sending verification e-mail."));
                return this.RedirectBack();
            }

            // Create all necessary information to send e-mail verification
            string callbackUrl = await this.SendEmailVerificationToken(user.Id);
#if !PROD
            // For testing this will show same link in next page, to ease confirmation procedure
            ViewBag.Link = callbackUrl;
#endif
            return this.View(MVC.Account.Views.DisplayEmailConfirmation);
        }

        /// <summary>
        /// Performs logoff operation for currently logged in user account
        /// </summary>
        /// <returns>To Home page</returns>
        [HttpGet]
        public virtual ActionResult LogOff()
        {
            this.AuthenticationManager.SignOut();
            return this.RedirectToAction(MVC.Home.Index());
        }

        /// <summary>
        /// Invokes a screen for user to be able to change password (sent via link in its account e-mail
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult ForgotPassword()
        {
            return this.View(MVC.Account.Views.ForgotPassword, new ForgotPasswordViewModel());
        }

        /// <summary>
        /// Accepts input from Forgot Password form (e-mail address) and sends encrupted link for it (if found) to get to change password form
        /// </summary>
        /// <param name="model">The model with e-mail address.</param>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(MVC.Account.Views.ForgotPassword, model);
            }

            var user = await this.UserManager.FindByNameAsyncWrap(model.EmailAddress);
            if (user == null || !(await this.UserManager.IsEmailConfirmedAsyncWrap(user.Id)))
            {
                // Don't reveal that the user does not exist or is not confirmed - all are "OK-ish" and "we sent link..."
                return this.View(MVC.Account.Views.ForgotPasswordConfirmation);
            }

            var code = await this.UserManager.GeneratePasswordResetTokenAsyncWrap(user.Id);
            var callbackUrl = this.Url.Action(MVC.Account.ActionNames.ResetPassword, MVC.Account.Name, new { userId = user.Id, code = code }, protocol: this.Request.Url.Scheme);
            await this.UserManager.SendEmailAsyncWrap(user.Id, "Finnish Immigration eService Reset Password request", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
            this.ViewBag.Link = callbackUrl;
            return this.View(MVC.Account.Views.ForgotPasswordConfirmation);
        }

        /// <summary>
        /// Loads the screen of Reset Password. Normally is invoked from sent e-mail link
        /// </summary>
        /// <param name="code">The validation code which is sent via e-mail to user.</param>
        [AllowAnonymous]
        public virtual ActionResult ResetPassword(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                // TODO: Add to master data translations!
                this.WebMessages.AddErrorMessage(this.T("Invalid entry"), this.T("reset password can be invoked only from link, sent in e-mail."));
                return this.RedirectBack();
            }

            var model = new ResetPasswordViewModel { Code = code };
            return this.View(MVC.Account.Views.ResetPassword, model);
        }

        /// <summary>
        /// Accepts the form data from Reset password screen (through Forgot password)
        /// </summary>
        /// <param name="model">The model of a view.</param>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(MVC.Account.Views.ResetPassword, model);
            }

            var user = await this.UserManager.FindByNameAsyncWrap(model.EmailAddress);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return this.RedirectToAction(MVC.Account.ResetPasswordConfirmation());
            }

            var result = await this.UserManager.ResetPasswordAsyncWrap(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return this.RedirectToAction(MVC.Account.ResetPasswordConfirmation());
            }

            foreach (var errorMessage in result.Errors)
            {
                this.WebMessages.AddErrorMessage(errorMessage);
            }

            return this.View(MVC.Account.Views.ResetPassword, model);
        }

        /// <summary>
        /// Loads a screen with password reset confirmation
        /// </summary>
        [AllowAnonymous]
        public virtual ActionResult ResetPasswordConfirmation()
        {
            return this.View(MVC.Account.Views.ResetPasswordConfirmation);
        }

        /// <summary>
        /// Loads screen for authorized users to change their password
        /// </summary>
        [HttpGet]
        public virtual ActionResult ChangePassword()
        {
            return this.View(MVC.Account.Views.ChangePassword);
        }

        /// <summary>
        /// Receives input from Change password screen and performs necessary logic based on this input
        /// </summary>
        /// <param name="model">Change password model</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(MVC.Account.Views.ChangePassword, model);
            }

            var result = await this.UserManager.ChangePasswordAsyncWrap(int.Parse(User.Identity.GetUserId()), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await this.UserManager.FindByIdAsyncWrap(int.Parse(User.Identity.GetUserId()));
                if (user != null)
                {
                    await this.SignInManager.RelogonUserAsyncWrap(user, false);
                }

                // TODO: Add master data translations for those
                this.WebMessages.AddSuccessMessage(this.T("Change password"), this.T("Password change was success"));
                return this.RedirectToAction(MVC.Account.UserProfile());
            }

            foreach (var errorMessage in result.Errors)
            {
                this.WebMessages.AddErrorMessage(errorMessage);
            }

            return this.View(MVC.Account.Views.ChangePassword, model);
        }

        /// <summary>
        /// Loads User Profile page (Read version)
        /// </summary>
        public async virtual Task<ActionResult> UserProfile()
        {
            WebUser userData = await this.UserManager.FindByIdAsyncWrap(int.Parse(User.Identity.GetUserId()));
            UserProfileModel model = userData.ToUserProfileModel();
            if (userData.IsStronglyAuthenticated)
            {
                return this.View(MVC.Account.Views.UserProfileStrong, model);
            }
            
            return this.View(MVC.Account.Views.UserProfile, model);
        }

        /// <summary>
        /// Calls and load user Profile page for its full field editing (first-time logon)
        /// </summary>
        [HttpGet]
        public async virtual Task<ActionResult> UserProfileFullEdit()
        {
            WebUser userData = await this.UserManager.FindByIdAsyncWrap(int.Parse(User.Identity.GetUserId()));
            UserProfileModel model = userData.ToUserProfileModel();
            return this.View(MVC.Account.Views.UserProfileFullEdit, model);
        }

        /// <summary>
        /// Receives posting User profile data
        /// </summary>
        /// <param name="model">Model of user profile additional data</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DatabaseTransaction]
        public virtual async Task<ActionResult> UserProfileFullSave(UserProfileModel model)
        {
            WebUser oldUserData = await this.UserManager.FindByIdAsyncWrap(int.Parse(User.Identity.GetUserId()));
            if (!ModelState.IsValid)
            {
                if (Request.UrlReferrer.LocalPath == "/account/userprofilefulledit")
                {
                    return this.View(MVC.Account.Views.UserProfileFullEdit, model);
                }

                if (oldUserData.IsStronglyAuthenticated)
                {
                    return this.View(MVC.Account.Views.UserProfileStrong, model);
                }

                return this.View(MVC.Account.Views.UserProfile, model);
            }

            bool isUserNameChanged = oldUserData.UserName != model.Email;
            WebUser userData = oldUserData.FromUserProfile(model);
            var result = await this.UserManager.UpdateAsyncWrap(int.Parse(User.Identity.GetUserId()), userData);
            IdentityResult emailResetResult = new IdentityResult();
            if (isUserNameChanged)
            {
                emailResetResult = await this.UserManager.SetEmailAsyncWrap(int.Parse(User.Identity.GetUserId()), model.Email);
            }

            if (result.Succeeded)
            {
                var user = await this.UserManager.FindByIdAsyncWrap(int.Parse(User.Identity.GetUserId()));
                if (user != null)
                {
                    await this.SignInManager.RelogonUserAsyncWrap(user, false);

                    // Check whether e-mail confirmation is required
                    if (isUserNameChanged && emailResetResult.Succeeded)
                    {
                        // Create all necessary information to send e-mail verification
                        string callbackUrl = await this.SendEmailVerificationToken(int.Parse(User.Identity.GetUserId()));

                        // Need to logoff user, so he is forced and guided to log in back again with new username/e-mail
                        this.AuthenticationManager.SignOut();
#if !PROD
                        // For testing this will show same link in next page, to ease confirmation procedure
                        ViewBag.Link = callbackUrl;
#endif
                        return this.View(MVC.Account.Views.ViewNames.DisplayEmailConfirmation);
                    }
                }

                // TODO: Add master data translations for those
                this.WebMessages.AddSuccessMessage(this.T("User profile"), this.T("User profile data changed succesfully"));
                return this.RedirectToAction(MVC.Account.UserProfile());
            }
            
            if (this.Request.UrlReferrer.LocalPath == "/account/userprofilefulledit")
            {
                return this.View(MVC.Account.Views.UserProfileFullEdit, model);
            }

            if (oldUserData.IsStronglyAuthenticated)
            {
                return this.View(MVC.Account.Views.UserProfileStrong, model);
            }

            return this.View(MVC.Account.Views.UserProfile, model);
        }

        /// <summary>
        /// Creates strongly authenticated user out of data from Vetuma and User Profile
        /// </summary>
        /// <param name="model">The model of profile with auto-added Vetuma properties.</param>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [DatabaseTransaction]
        public virtual async Task<ActionResult> StrongUserCreate(UserProfileModel model)
        {
            if (model == null)
            {
                // TODO: Add master data translations for those
                this.WebMessages.AddErrorMessage(this.T("User profile"), this.T("User profile data was not present in request"));
                return this.RedirectBack();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(MVC.Account.Views.UserProfileStrongEdit, model);
            }

            var existingUser = await this.UserManager.FindByEmailAsyncWrap(model.Email);
            if (existingUser != null)
            {
                // TODO: Add master data translations for those
                this.WebMessages.AddInfoMessage(this.T("User account"), this.T("User with given e-mail address already exists. Please, signIn with e-mail address and password and then upgrade account."));
                return this.RedirectToAction(MVC.Account.Login());
            }

            var result = await this.UserManager.CreateAsyncWrap(model.Email, null);
            if (result.Item1.Succeeded && result.Item2 != 0)
            {
                WebUser user = new WebUser().FromUserProfile(model);
                user.Id = result.Item2;
                user.IsStronglyAuthenticated = true;
                var updateResult = await this.UserManager.UpdateAsyncWrap(result.Item2, user);
                if (updateResult.Succeeded)
                {
                    // Sign in Vetuma user automatically
                    await this.SignInManager.SignInAsyncWrap(user, true, false);

                    // Create all necessary information to send e-mail verification
                    string callbackUrl = await this.SendEmailVerificationToken(user.Id);
#if !PROD
                    // For testing this will show same link in next page, to ease confirmation procedure
                    ViewBag.Link = callbackUrl;
#endif
                    return this.View(MVC.Account.Views.DisplayEmailConfirmation);
                }

                return this.RedirectBack();
            }

            foreach (var errorMessage in result.Item1.Errors)
            {
                this.WebMessages.AddErrorMessage(errorMessage);
            }

            return this.View(MVC.Account.Views.RegistrationForm, model);
        }
    }
}