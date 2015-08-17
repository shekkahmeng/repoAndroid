using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ConferenceManagementSystem.Startup))]
namespace ConferenceManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
