using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SlnBlogAndShop.Startup))]
namespace SlnBlogAndShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureSignalR(app);
        }
    }
}
