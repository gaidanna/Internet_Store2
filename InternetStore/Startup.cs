using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InternetStore.Startup))]
namespace InternetStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
