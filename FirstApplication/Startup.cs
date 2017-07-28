using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FirstApplication.Startup))]
namespace FirstApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
