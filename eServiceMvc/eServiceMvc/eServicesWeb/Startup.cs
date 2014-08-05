using Microsoft.Owin;
using Owin;
using Uma.Eservices.Web.Components;

[assembly: OwinStartupAttribute(typeof(Uma.Eservices.Web.Startup))]

namespace Uma.Eservices.Web
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// OWIN startup file to initiate Application level contexts, variables and configurations of ones
    /// </summary>
    [ExcludeFromCodeCoverage]
    public partial class Startup
    {
        /// <summary>
        /// Configurate the specified application - Magic!
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
#if DEBUG
            app.UseErrorPage();
#endif
            DependencyConfiguration();
            this.ConfigureAuth(app);
        }
    }
}