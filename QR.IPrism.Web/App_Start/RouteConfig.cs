using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QR.IPrism.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{reject}", new { reject = @"(.*/)?Rejected-By-UrlScan(/.)?" });

            routes.MapRoute(
               name: "Unauthorized",
               url: "Unauthorized",
               defaults: new { controller = "Main", action = "Unauthorized" }
           );

            routes.MapRoute(
            name: "Error",
            url: "Error",
            defaults: new { controller = "Main", action = "Error" }
            );

            routes.MapRoute(
             name: "TimeOut",
            url: "TimeOut",
            defaults: new { controller = "Main", action = "TimeOut" }
             );

            routes.MapRoute(
               name: "Logout",
               url: "Logout",
               defaults: new { controller = "Main", action = "Logout" }
           );

            routes.MapRoute(
             name: "Login",
             url: "Login",
             defaults: new { controller = "Login", action = "Login" }
         );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "ct", id = UrlParameter.Optional }
            );

          
        }
    }
}
