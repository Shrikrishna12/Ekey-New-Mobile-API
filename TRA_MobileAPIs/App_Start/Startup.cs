using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using TRA_MobileAPIs.Authorization;
using Owin;
using System;
using System.Web.Http;
using TRA_MobileAPIs.ConfigSettings;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(TRA_MobileAPIs.App_Start.Startup))]

namespace TRA_MobileAPIs.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
         
          

            var myProvider = new AuthorizationServerProvider();
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
                Provider = myProvider,
                RefreshTokenProvider = new RefreshTokenProvider()
            };
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);

            
            OrganizationService organization = OrganizationService.GetInstance();
        }
    }
}