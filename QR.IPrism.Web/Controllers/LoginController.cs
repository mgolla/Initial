using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using QR.IPrism.Security.Authentication;
using QR.IPrism.Web.API.Controllers.Shared;
using QR.IPrism.Web.Helper;
using QR.IPrism.Web.Models;

namespace QR.IPrism.Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["isLoginEnabled"].ToString()))
                return View();
            else
                return RedirectToRoute("Unauthorized");
        }
        [HttpPost]
        public ActionResult Post(LoginModel loginModel)
        {
            try
            {
                string LoggedInUser = loginModel.StaffNumber;
                if (loginModel != null && loginModel.Password != null && !loginModel.Password.Equals(ConfigurationManager.AppSettings["Password"]))
                    return Json(new { IsSuccess = false, Messsage = "The staff id or password you entered is incorrect." });
                var securityManager = DependencyResolver.Current.GetService<SecurityManager>();
                bool isSuccess = securityManager.SetLoggedinUserContext(loginModel.StaffNumber);
                if (isSuccess)
                {
                    return Json(new { IsSuccess = true, IsCaptchaEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["isCaptchEnabled"].ToString()), Messsage = loginModel.StaffNumber });
                }
                else
                    return Json(new { IsSuccess = false, IsCaptchaEnabled=false, Messsage = loginModel.StaffNumber });
            }
            catch (System.Exception exception)
            {
                return Json(new { IsSuccess = false, Messsage = exception.Message.ToString() });
            }
        }
    }
}