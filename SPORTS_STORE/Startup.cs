using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SPORTS_STORE.Startup))]
namespace SPORTS_STORE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
