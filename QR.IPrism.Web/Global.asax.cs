using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using QR.IPrism.Adapter.Helper;
using QR.IPrism.Security;
using QR.IPrism.Security.Authentication;
using QR.IPrism.Web.Common;
using QR.IPrism.Web.Helper;

namespace QR.IPrism.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ConfigEncryption();
            AutoMapperConfiguration.configureClientMapping();
            MvcHandler.DisableMvcResponseHeader = true;
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //var app = (HttpApplication)sender;
            //string path = app.Context.Request.Url.PathAndQuery;
            //ISecurityManager manager = new SecurityManager();
            //if (path.IndexOf(Constants.Cordova, StringComparison.OrdinalIgnoreCase) > -1)
            //    manager.SetCookie("true", ScConstants.CordovaCookieName);
        }

        private void ConfigEncryption()
        {
            if (!HttpContext.Current.IsDebuggingEnabled)
            {
                RsaEncryption.EncryptConnString("connectionStrings");
                RsaEncryption.EncryptConnString("appSettings");
            }
            else
            {
                RsaEncryption.DecryptConnString("connectionStrings");
                RsaEncryption.DecryptConnString("appSettings");
            }
        }
        protected void Session_Start()
        {

        }
        //public static void RegisterWebApiFilters(System.Web.Http.Filters.HttpFilterCollection filters)
        //{
        //    filters.Add(new ValidateTokenAttribute());
        //}

        //protected void Application_PreSendRequestHeaders()
        //{
        //    Response.Headers.Remove("Server");
        //    //Response.Headers.Set("Server", "MNA");
        //    Response.Headers.Remove("X-AspNet-Version"); //alternative to above solution
        //    Response.Headers.Remove("X-AspNetMvc-Version"); //alternative to above solution
        //}
    }
}
    