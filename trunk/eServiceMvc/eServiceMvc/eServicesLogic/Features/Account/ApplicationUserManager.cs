namespace Uma.Eservices.Logic.Features.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Models.Account;

    /// <summary>
    /// Wrapper of UserManager of Identity Framework, containing its configuration and User management procedures
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser, int>, IApplicationUserManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserManager"/> class.
        /// </summary>
        /// <param name="store">The store object (with DB context) to perform persistence functions.</param>
        public ApplicationUserManager(IUserStore<ApplicationUser, int> store)
            : base(store)
        {
        }

        /// <summary>
        /// Creates the User Manager used by OWIN to create it when necessary.
        /// </summary>
        /// <param name="options">The options from OWIN.</param>
        /// <param name="context">The context of OWIN.</param>
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new ApplicationUserStore(context.Get<DatabaseContext>())) { PasswordHasher = new CompatiblePasswordHasher() };

            // What usernames are accepted by Identity
            manager.UserValidator = new UserValidator<ApplicationUser, int>(manager) { AllowOnlyAlphanumericUserNames = false, RequireUniqueEmail = true };

            // Password strenght definition
            manager.PasswordValidator = new PasswordValidator()
                                            {
                                                RequiredLength = 8,
                                                RequireNonLetterOrDigit = false,
                                                RequireDigit = true,
                                                RequireLowercase = true,
                                                RequireUppercase = true,
                                            };

            // Browser allowance (second factor authentication) provider config
            manager.RegisterTwoFactorProvider(
                "EmailCode",
                new EmailTokenProvider<ApplicationUser, int>
                    {
                        Subject = "Migri eService browser access code",
                        BodyFormat = "Your security code for browser session is {0}"
                    });

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // manager.SmsService = new SmsService();
            manager.EmailService = new EmailService();

            // Validation and authentication code encryptors/generators
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, int>(dataProtectionProvider.Create("UMA eService Token"))
                                                {
                                                    TokenLifespan =
                                                        TimeSpan
                                                        .FromHours(
                                                            24)
                                                };
            }

            return manager;
        }

        /// <summary>
        /// Generates the user identity asynchronously.
        /// </summary>
        /// <param name="user">The user object.</param>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUser user)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await this.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        /// <summary>
        /// Generates the user identity asynchronously.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(int userId)
        {
            var user = await this.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            var userIdentity = await this.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        /// <summary>
        /// Creates the user in database, based on current known data (e-mail and password)
        /// </summary>
        /// <param name="eMail">The e mail of a user.</param>
        /// <param name="password">The password, user specified.</param>
        /// <returns>Tuple result, Item1 = <see cref="IdentityResult"/>, Item2 = Created User ID</returns>
        public async Task<Tuple<IdentityResult, int>> CreateAsyncWrap(string eMail, string password)
        {
            ApplicationUser user = new ApplicationUser { UserName = eMail, Email = eMail };
            IdentityResult ident;
            if (string.IsNullOrEmpty(password))
            {
                // Strong authentication users doesnt have passwords
                ident = await this.CreateAsync(user);
            }
            else
            {
                ident = await this.CreateAsync(user, password);
            }
            
            return new Tuple<IdentityResult, int>(ident, user.Id);
        }

        /// <summary>
        /// Retrieve list of defined providers (SMS/e-mail) for sending security code
        /// </summary>
        /// <param name="userId">ID of a user in question</param>
        /// <returns>List of verification code send options (SMS/Email)</returns>
        public async Task<IList<string>> GetValidTwoFactorProvidersAsyncWrap(int userId)
        {
            return await this.GetValidTwoFactorProvidersAsync(userId);
        }

        /// <summary>
        /// Retrieves Token of E-mail confirmation to send it via e-mail to get user verification of registration
        /// </summary>
        /// <param name="userId">ID of a user in question</param>
        /// <returns>Verification code (encrypted)</returns>
        public async Task<string> GenerateEmailConfirmationTokenAsyncWrap(int userId)
        {
            return await this.GenerateEmailConfirmationTokenAsync(userId);
        }

        /// <summary>
        /// Wrapper for e-mail send service for OWIN / Identity use.
        /// </summary>
        /// <param name="userId">ID of a user</param>
        /// <param name="subject">Subject o a e-mail</param>
        /// <param name="body">Body (message itself)</param>
        public async Task SendEmailAsyncWrap(int userId, string subject, string body)
        {
            await this.SendEmailAsync(userId, subject, body);
        }

        /// <summary>
        /// Returns UI model fro user counterpart in security persistence
        /// </summary>
        /// <param name="userId">ID of persisted user</param>
        public async Task<WebUser> FindByIdAsyncWrap(int userId)
        {
            var user = await this.FindByIdAsync(userId);
            return this.MapToWebUser(user);
        }

        /// <summary>
        /// Returns Toke (Code) for two-factor authentication (Browser access allowance)
        /// </summary>
        /// <param name="userId">ID of a user</param>
        /// <param name="sendProviderName">Provider name (SMSM or e-mail)</param>
        public async Task<string> GenerateTwoFactorTokenAsyncWrap(int userId, string sendProviderName)
        {
            return await this.GenerateTwoFactorTokenAsync(userId, sendProviderName);
        }

        /// <summary>
        /// E-mail confirmation method wrapper
        /// </summary>
        /// <param name="userId">ID of a user</param>
        /// <param name="confirmationCode">Confirmation code</param>
        public async Task<IdentityResult> ConfirmEmailAsyncWrap(int userId, string confirmationCode)
        {
            return await this.ConfirmEmailAsync(userId, confirmationCode);
        }

        /// <summary>
        /// Returns User data by its UserName (email)
        /// </summary>
        /// <param name="emailAddress">Email address (which is username)</param>
        public async Task<WebUser> FindByNameAsyncWrap(string emailAddress)
        {
            ApplicationUser appUser = await FindByNameAsync(emailAddress);
            return this.MapToWebUser(appUser);
        }

        /// <summary>
        /// Returns User data by its UserName (email)
        /// </summary>
        /// <param name="emailAddress">Email address (which is username)</param>
        public async Task<WebUser> FindByEmailAsyncWrap(string emailAddress)
        {
            ApplicationUser appUser = await FindByNameAsync(emailAddress);
            return this.MapToWebUser(appUser);
        }

        /// <summary>
        /// Determines whether is email confirmed for the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public async Task<bool> IsEmailConfirmedAsyncWrap(int userId)
        {
            return await this.IsEmailConfirmedAsync(userId);
        }

        /// <summary>
        /// Switch for Two-Factor authentication for user profile.
        /// Should be used for weak authenticated users, turn off for strong authentication users
        /// </summary>
        /// <param name="userId">Identifier of a user</param>
        /// <param name="secondFactorSwitch">If true - will turn on, false - turn off</param>
        public async Task<IdentityResult> SetTwoFactorEnabledAsyncWrap(int userId, bool secondFactorSwitch)
        {
            return await this.SetTwoFactorEnabledAsync(userId, secondFactorSwitch);
        }

        /// <summary>
        /// Wrapper around create Reset password token for specific user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Reset password token as string</returns>
        public async Task<string> GeneratePasswordResetTokenAsyncWrap(int userId)
        {
            return await this.GeneratePasswordResetTokenAsync(userId);
        }

        /// <summary>
        /// Resets the user password, if token is correct for him
        /// </summary>
        /// <param name="userId">User unique identifier</param>
        /// <param name="resetToken">Token that has been sent to user in e-mail link</param>
        /// <param name="newPassword">New password which user entered in Reset password screen</param>
        public async Task<IdentityResult> ResetPasswordAsyncWrap(int userId, string resetToken, string newPassword)
        {
            return await this.ResetPasswordAsync(userId, resetToken, newPassword);
        }

        /// <summary>
        /// Changes the user's password asynchronously
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        public async Task<IdentityResult> ChangePasswordAsyncWrap(int userId, string oldPassword, string newPassword)
        {
            return await this.ChangePasswordAsync(userId, oldPassword, newPassword);
        }

        /// <summary>
        /// Updates the application user asynchronously.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userData">The user data from profile.</param>
        public async Task<IdentityResult> UpdateAsyncWrap(int userId, WebUser userData)
        {
            var user = await this.FindByIdAsync(userId);
            user.FirstName = userData.FirstName;
            user.LastName = userData.LastName;
            user.BirthDate = userData.BirthDate;
            user.PhoneNumber = userData.Mobile;
            user.Email = userData.UserName;
            user.UserName = userData.UserName;
            user.PersonCode = userData.PersonCode;
            user.IsStronglyAuthenticated = userData.IsStronglyAuthenticated;
            return await this.UpdateAsync(user);
        }

        /// <summary>
        /// Resets User e-mail address to other, resetting Verification token
        /// </summary>
        /// <param name="userId">Unique identifier of a user</param>
        /// <param name="emailAddress">New e-mail address</param>
        /// <returns>Result of operation</returns>
        public async Task<IdentityResult> SetEmailAsyncWrap(int userId, string emailAddress)
        {
            return await this.SetEmailAsync(userId, emailAddress);
        }

        /// <summary>
        /// Transforms Persistence ApplicationUser to UI WebUser
        /// </summary>
        /// <param name="user">The user data in persistence context.</param>
        /// <returns>WebUser in UI context</returns>
        private WebUser MapToWebUser(ApplicationUser user)
        {
            if (user == null)
            {
                return null;
            }

            return new WebUser
                       {
                           Id = user.Id,
                           FirstName = user.FirstName,
                           LastName = user.LastName,
                           UserName = user.UserName,
                           BirthDate = user.BirthDate,
                           PersonCode = user.PersonCode,
                           CustomerId = user.CustomerId,
                           Mobile = user.PhoneNumber,
                           IsStronglyAuthenticated = user.IsStronglyAuthenticated
                       };
        }

        /// <summary>
        /// Retrieves WebUser data from database of users by his PersonCode (HETU)
        /// </summary>
        /// <param name="personCode">Finnish PersonCode (HETU)</param>
        /// <returns>WebUser or NULL if not found</returns>
        public WebUser FindByPersonCodeStrong(string personCode)
        {
            ApplicationUser dbUser = this.Users.FirstOrDefault(u => u.PersonCode == personCode);
            return this.MapToWebUser(dbUser);
        }
    }
}
