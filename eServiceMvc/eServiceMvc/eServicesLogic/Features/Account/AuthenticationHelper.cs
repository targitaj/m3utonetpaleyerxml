namespace Uma.Eservices.Logic.Features.Account
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Owin;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Helper functions to achieve layer separation
    /// </summary>
    public static class AuthenticationHelper
    {
        /// <summary>
        /// Configures APP of OWIN to add necessary object creation methods for it
        /// </summary>
        /// <param name="app">The application object</param>
        public static void AddOwinContextObjects(IAppBuilder app)
        {
            app.CreatePerOwinContext<DatabaseContext>(DatabaseContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
        }

        /// <summary>
        /// Configures the cookie for OWIN Authentication configuration.
        /// </summary>
        /// <param name="app">The application object.</param>
        public static void ConfigureCookie(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser, int>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentityCallback: RegenerateIdentityCallback,
                        getUserIdCallback: (user) => int.Parse(user.GetUserId()))
                },
                CookieSecure = CookieSecureOption.Always
            });
        }

        /// <summary>
        /// Regenerates the identity callback function for cookie configuration (above).
        /// </summary>
        /// <param name="applicationUserManager">The application user manager.</param>
        /// <param name="applicationUser">The application user.</param>
        private static async Task<ClaimsIdentity> RegenerateIdentityCallback(ApplicationUserManager applicationUserManager, ApplicationUser applicationUser)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await applicationUserManager.CreateIdentityAsync(applicationUser, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
