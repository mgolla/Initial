using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using QR.IPrism.Security.Authentication;
using RestSharp;

namespace QR.IPrism.Web.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var securityManager = DependencyResolver.Current.GetService<SecurityManager>();
            if (securityManager.GetLoggedinUserContext() == null)
            {
                RedirectToRoute("Unauthorized");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}