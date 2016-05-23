using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BUGTRACKER.Startup))]
namespace BUGTRACKER
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
