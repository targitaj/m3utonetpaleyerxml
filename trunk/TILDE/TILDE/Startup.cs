using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TILDE.Startup))]
namespace TILDE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
