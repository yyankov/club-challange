using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;

namespace ClubChallengeBeta
{
    public partial class Startup
    {
        #region static
        static string microsoftId = ConfigurationManager.AppSettings.Get("microsoftId");
        static string microsoftKey = ConfigurationManager.AppSettings.Get("microsoftKey");
        static string facebookId = ConfigurationManager.AppSettings.Get("facebookId");
        static string facebookKey = ConfigurationManager.AppSettings.Get("facebookKey");
        static string googleId = ConfigurationManager.AppSettings.Get("googleId");
        static string googleKey = ConfigurationManager.AppSettings.Get("googleKey");
        #endregion


        public void ConfigureAuth(IAppBuilder app)
        {

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseMicrosoftAccountAuthentication(
                clientId: microsoftId,
                clientSecret: microsoftKey);


            //app.UseFacebookAuthentication(
            //    appId: facebookId,
            //    appSecret: facebookKey);

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = googleId,
                ClientSecret = googleKey,
                Provider = new GoogleOAuth2AuthenticationProvider()
            });
        }
    }
}