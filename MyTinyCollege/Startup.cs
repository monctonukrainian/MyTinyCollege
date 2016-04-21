using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyTinyCollege.Startup))]
namespace MyTinyCollege
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
