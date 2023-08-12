using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TheVaqeel.Website.Startup))]
namespace TheVaqeel.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
