using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;

namespace ClubChallengeBeta
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            // Uncomment the following lines to enable logging in with third party login providers
            app.UseMicrosoftAccountAuthentication(
                clientId: "0000000044151A29",
                clientSecret: "3yiAaWL7detOCYyXQYlcPqgXl9wldC80");


            app.UseTwitterAuthentication(
               consumerKey: "QdQTTvZqu4cqyqfOQ4iqGeeLV ",
               consumerSecret: "snSWZaT3CImCUfYMn9yhStERMQi3uMyPTvk5kUmLuYkV5Jcq4j");


            app.UseFacebookAuthentication(
                appId: "372763689600824",
                appSecret: "f858477b14730957b76b552a36102a76");

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "1051623227327-raujnh43rfcv5t76bnh7anee951rq4jc.apps.googleusercontent.com",
                ClientSecret = "q8PMpoLwHC8I_W9IZhN0DN1u",
                Provider = new GoogleOAuth2AuthenticationProvider()
            });

            //app.UseGoogleAuthentication();
        }
    }
}