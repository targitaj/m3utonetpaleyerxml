using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DPA.Startup))]
namespace DPA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
