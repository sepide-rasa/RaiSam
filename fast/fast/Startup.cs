using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(fast.Startup))]
namespace fast
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
