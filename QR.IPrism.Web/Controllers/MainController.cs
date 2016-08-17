using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using QR.IPrism.Security.Authentication;
using CaptchaMvc.HtmlHelpers;
using QR.IPrism.Models.Shared;
using QR.IPrism.Web.Helper;

namespace QR.IPrism.Web.Controllers
{
    public class MainController : Controller
    {
        ISecurityManager _securityManager;
        public MainController()
        {
            _securityManager = DependencyResolver.Current.GetService<SecurityManager>();
        }
        [ActionName("hm")]
        public ActionResult Home()
        {
            UserContextModel cotext = _securityManager.GetLoggedinUserContext();
            if(cotext != null)
            {
                if(!Convert.ToBoolean(ConfigurationManager.AppSettings["isCaptchEnabled"].ToString()))
                    return View("IpmHome", "_IpmLayout");
                else if(cotext.IsCaptchaFilled)
                    return View("IpmHome", "_IpmLayout");
                else
                    return RedirectToRoute("Unauthorized");
            }
            else
                return RedirectToRoute("Unauthorized");
        }
        [ActionName("ct")]
        public ActionResult Captcha()
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["isLoginEnabled"].ToString()))
                return RedirectToRoute("Login");
            else
            {
                bool isSuccess = _securityManager.SetLoggedinUserContext(Request.ServerVariables["LOGON_USER"].Split(new char[] { '\\' }).Last());
                if (!isSuccess)
                    return RedirectToRoute("Unauthorized");
                else
                    return View("Captcha");
            }
        }
        [ActionName("cta")]
        public ActionResult CaptchaLgn()
        {
            UserContextModel cotext = _securityManager.GetLoggedinUserContext();
            if (cotext != null && !string.IsNullOrEmpty(cotext.StaffNumber))
                return View("Captcha");
            else
                return RedirectToRoute("Unauthorized");
        }
        [HttpPost]
        public ActionResult CaptchaPost(string userPin)
        {
            if (this.IsCaptchaValid(ConfigurationManager.AppSettings["CaptchaNotValid"].ToString()))
            {
                UserContextModel cotext = _securityManager.GetLoggedinUserContext();
                if ((cotext.Role.FirstOrDefault().Name.Equals(Constants.Admin, StringComparison.OrdinalIgnoreCase) && userPin.Equals(cotext.AdminKey))
                    || (cotext.JoiningDate.Replace("/", "").Equals(userPin)))
                {
                    bool isValid = _securityManager.UpdateUserContextCaptcha(true);
                    if (isValid)
                        return RedirectToAction("hm");
                    else
                        return RedirectToRoute("Unauthorized");
                }
                else
                {
                    ViewBag.ErrMessage = ConfigurationManager.AppSettings["PinErrMsg"].ToString();
                    return View("Captcha");
                }
            }
            ViewBag.ErrMessage = ConfigurationManager.AppSettings["CaptchaErrMsg"].ToString();
            return View("Captcha");
        }
        public ActionResult TimeOut()
        {
            _securityManager.ClearSecurityContext();
            if (System.Web.HttpContext.Current.Request.Url.ToString().IndexOf("localhost") > -1)
                return View("Timeout");
            else
                return Redirect("/_layouts/SignOut.aspx");
        }
        public ActionResult Logout()
        {
            _securityManager.ClearSecurityContext();
            if (System.Web.HttpContext.Current.Request.Url.ToString().IndexOf("localhost") > -1)
                return View("Logout");
            else
                return Redirect("/_layouts/SignOut.aspx");
        }
        public ActionResult Unauthorized()
        {
            var securityManager = DependencyResolver.Current.GetService<SecurityManager>();
            securityManager.ClearSecurityContext();
            return View("Unauthorized");
        }
        public ActionResult Error()
        {
            var securityManager = DependencyResolver.Current.GetService<SecurityManager>();
            securityManager.ClearSecurityContext();
            return View("Error");
        }
    }
}