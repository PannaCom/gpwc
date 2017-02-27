using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(gpw.Startup))]
namespace gpw
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
