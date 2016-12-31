using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PetsAPI.Startup))]
namespace PetsAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
