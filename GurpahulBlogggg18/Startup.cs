using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GurpahulBlogggg18.Startup))]
namespace GurpahulBlogggg18
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
