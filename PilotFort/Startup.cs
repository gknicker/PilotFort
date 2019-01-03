using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PilotFort.Startup))]
namespace PilotFort
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
