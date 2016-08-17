using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Elmah;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using QR.IPrism.Adapter.Helper;
using QR.IPrism.Security;
using Unity.WebApi;

[assembly: OwinStartup(typeof(QR.IPrism.Web.API.Startup))]
namespace QR.IPrism.Web.API
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static OAuthAuthorizationServerOptions OAuthServerOptions { get; private set; }
        public void Configuration(IAppBuilder app)
        {
            //OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            //OAuthServerOptions = new OAuthAuthorizationServerOptions();
            //ConfigureOAuth(app);

            HttpConfiguration config = new HttpConfiguration();
            var resolver = new UnityDependencyResolver(UnityConfig.GetConfiguredContainer());
            config.DependencyResolver = resolver;
            config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthBearerOptions.AuthenticationType));
            //config.Filters.Add(new HostAuthenticationFilter(OAuthServerOptions.AuthenticationType));
            WebApiConfig.Register(config);
            app.UseWebApi(config);
            AutoMapperConfiguration.configureClientMapping();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
        }
        public void ConfigureOAuth(IAppBuilder app)
        {

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                Provider = new AuthorizationServerProvider(),
                RefreshTokenProvider=new RefreshTokenProvider()
            };
            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);

            //app.Use(typeof(GlobalExceptionMiddleware));
            
            
           
        }
    }
}