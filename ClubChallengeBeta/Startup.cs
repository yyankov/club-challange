using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClubChallengeBeta.Startup))]
namespace ClubChallengeBeta
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
