using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PokeMap.Startup))]
namespace PokeMap
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
