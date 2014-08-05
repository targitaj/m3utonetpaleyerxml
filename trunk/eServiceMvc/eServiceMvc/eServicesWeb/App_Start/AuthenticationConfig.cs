namespace Uma.Eservices.Web
{
    using System;
    using System.Management.Instrumentation;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Helpers;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Practices.Unity;
    using Owin;
    using Uma.Eservices.Logic.Features.Account;
    using Uma.Eservices.Models.Account;
    using Uma.Eservices.Web.Core;

    /// <summary>
    /// OWIN (Katana) startup file extension (partial class)
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configures the authentication for Asp.Net application through OWIN (Katana)
        /// </summary>
        /// <param name="app">The application builder.</param>
        public void ConfigureAuth(IAppBuilder app)
        {
            AuthenticationHelper.AddOwinContextObjects(app);

            // Enable the application to use a cookie to store information for the signed in user
            AuthenticationHelper.ConfigureCookie(app);
            
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Set Uniq claim idetifier
            // See: http://brockallen.com/2012/07/08/mvc-4-antiforgerytoken-and-claims/
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}