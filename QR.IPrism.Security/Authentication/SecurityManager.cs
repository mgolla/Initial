using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using QR.IPrism.Adapter.Implementation;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.Models.Shared;
using QR.IPrism.Utility;
using RestSharp.Deserializers;


namespace QR.IPrism.Security.Authentication
{
   
    public class SecurityManager : ISecurityManager
    {
        ISharedAdapter _srdAdapter = null;

        public SecurityManager()
        {
            _srdAdapter = new SharedAdapter();
        }
        /// <summary>
        /// Generate the token
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool SetLoggedinUserContext(string userName)
        {
            UserContextModel model = _srdAdapter.GetUserContext(userName);
            if (model != null)
            {
                SetLoggedinUserSecurityContext(model,false);
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// Generate the token
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string SetImpersoneteUserContext(string userName)
        {
            UserContextModel model = _srdAdapter.GetUserContext(userName);
            if (model != null)
            {
                if(model.IsAdmin)
                    return "IMPERSNTADMERROR";
                model.IsCaptchaFilled = true;
                SetLoggedinUserSecurityContext(model,true);
                AnalyticModel antcModel = new AnalyticModel();
                //_srdAdapter.CreateAnalyticEntryAsync();
                return string.Empty;
            }
            else
                return "INVALIDUSR";
        }
        /// <summary>
        /// Sets the logged in users Security Context
        /// </summary>
        /// <param name="context"></param>
        private void SetLoggedinUserSecurityContext(UserContextModel context, bool isImpersonation)
        {
            if (isImpersonation)
            {
                var model = GetLoggedinUserContext();
                context.ImpersonatedBy = model.StaffNumber;
                context.ImpersonatedByDtlsId = model.StaffNumber;
                context.ImpersonatedByUsrId = model.StaffNumber;
                context.ImpersonatedByName = model.StaffNumber;
            }
            if (System.Web.HttpContext.Current.Request.Cookies[ScConstants.AuthContext] != null)
            {
                System.Web.HttpContext.Current.Response.Cookies.Clear();
                HttpCookie c = new HttpCookie(ScConstants.AuthContext) { Expires = DateTime.Now.AddDays(-1) };
                System.Web.HttpContext.Current.Response.Cookies.Add(c);
            }
            var outputData = ObjectToBase64(context);
            var textToOutPut = Encoding.UTF8.GetBytes(outputData);
            var encryptedValue = Convert.ToBase64String(MachineKey.Protect(textToOutPut, ScConstants.AuthContextPurpose));

            HttpCookie cookieObject = new HttpCookie(ScConstants.AuthContext, encryptedValue);
            cookieObject.HttpOnly = true;
            System.Web.HttpContext.Current.Response.Cookies.Add(cookieObject);

        }

        /// <summary>
        /// To get the logged in users Security Context
        /// </summary>
        /// <returns></returns>
        public UserContextModel GetLoggedinUserContext()
        {
            UserContextModel context = null;
            if (System.Web.HttpContext.Current.Request.Cookies[ScConstants.AuthContext] != null)
            {
                var bytes = Convert.FromBase64String(System.Web.HttpContext.Current.Request.Cookies[ScConstants.AuthContext].Value);
                byte[] output = MachineKey.Unprotect(bytes, ScConstants.AuthContextPurpose);
                string originalData = Encoding.UTF8.GetString(output);
                context = Base64ToObject(originalData) as UserContextModel;
            }
            return context;
        }
         /// <summary>
        /// Set the Cookie
        /// </summary>
        /// <param name="context"></param>
        public void SetCookie(string context, string coockieName)
        {

            if (System.Web.HttpContext.Current.Request.Cookies[coockieName] != null)
            {
                System.Web.HttpContext.Current.Response.Cookies.Clear();
                HttpCookie c = new HttpCookie(coockieName) { Expires = DateTime.Now.AddDays(-1) };
                System.Web.HttpContext.Current.Response.Cookies.Add(c);
            }
            var outputData = ObjectToBase64(context);
            var textToOutPut = Encoding.UTF8.GetBytes(outputData);
            var encryptedValue = Convert.ToBase64String(MachineKey.Protect(textToOutPut, "NA"));

            HttpCookie cookieObject = new HttpCookie(coockieName, encryptedValue);
            cookieObject.HttpOnly = true;
            System.Web.HttpContext.Current.Response.Cookies.Add(cookieObject);
        }
        /// <summary>
        /// To get the logged in users Security Context
        /// </summary>
        /// <returns></returns>
        public string GetCookie(string coockieName)
        {
            string context = string.Empty;
            if (System.Web.HttpContext.Current.Request.Cookies[coockieName] != null)
            {
                var bytes = Convert.FromBase64String(System.Web.HttpContext.Current.Request.Cookies[coockieName].Value);
                byte[] output = MachineKey.Unprotect(bytes, "NA");
                string originalData = Encoding.UTF8.GetString(output);
                context = Base64ToObject(originalData).ToString();
            }
            return context;
        }
        public bool UpdateUserContextCaptcha(bool isCatpchaFilled)
        {
            try
            {
                UserContextModel context = GetLoggedinUserContext();
                if (context != null)
                {
                    context.IsCaptchaFilled = isCatpchaFilled;
                    SetLoggedinUserSecurityContext(context,false);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Generate the token
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private RestSharp.IRestResponse GetAuthToken(string userName)
        {
            RestSharp.RestClient restClient = new RestSharp.RestClient(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath);
            RestSharp.IRestRequest request = new RestSharp.RestRequest("token", RestSharp.Method.POST);
            string details = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(ConfigurationManager.AppSettings["usr"].ToString() + ":" + ConfigurationManager.AppSettings["pwd"].ToString()));
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", Common.Encrypt(userName, ScConstants.UserIdKey));
            request.AddParameter("client_Id", ScConstants.ClientId);
            request.AddParameter("client_Secret", ScConstants.ClientKey);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            return restClient.Execute(request); ;
            
        }
        /// <summary>
        /// Generate the token
        /// </summary>
        /// <param name="userName"></param> 
        /// <returns></returns>
        private RestSharp.IRestResponse GetRefreshAuthToken(string context)
        {
            RestSharp.RestClient restClient = new RestSharp.RestClient(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath);
            restClient.AddHandler("my_custom_type", new JsonDeserializer());
            RestSharp.IRestRequest request = new RestSharp.RestRequest("token", RestSharp.Method.POST);
            string details = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(ConfigurationManager.AppSettings["usr"].ToString() + ":" + ConfigurationManager.AppSettings["pwd"].ToString()));
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("refresh_token", context);
            request.AddParameter("client_id", ScConstants.ClientId);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            return restClient.Execute(request);
        }
       
        /// <summary>
        /// Generate the token
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool SetUserContext(string userName)
        {
            RestSharp.IRestResponse response = GetAuthToken(userName);
      
            if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized)
                return false;
            else
            {
                string content = response.Content.Replace("as:", string.Empty);
                JavaScriptSerializer jsonserializer = new JavaScriptSerializer();
                AuthContext user = jsonserializer.Deserialize<AuthContext>(content);
                if (user.Access_Token == null)
                    return false;
                user.LoggedInUser = userName;
                SetSecurityContext(user);
                return true;
            }
        }
        /// <summary>
        /// Generate the token
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public AuthContext SetImpUserContext(string userName)
        {
            AuthContext context = GetSecurityContext();
            if (context != null)
                _srdAdapter.RemoveUserTokenAsync(Common.Encrypt(context.refresh_token, SecuredConstants.TokenIdKey));

            RestSharp.IRestResponse response = GetAuthToken(userName);
            if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized)
                return null;
            else
            {
                string content = response.Content.Replace("as:", string.Empty);
                JavaScriptSerializer jsonserializer = new JavaScriptSerializer();
                AuthContext user = jsonserializer.Deserialize<AuthContext>(content);
                if (user.Access_Token == null)
                    return null;
                user.LoggedInUser = userName;
                SetSecurityContext(user);
                return user;
            }
        }
        public AuthContext SetContext(string content)
        {
            content = content.Replace("as:", string.Empty);
            JavaScriptSerializer jsonserializer = new JavaScriptSerializer();
            AuthContext user = jsonserializer.Deserialize<AuthContext>(content);
            if (user.Access_Token == null)
                return null;
            SetSecurityContext(user);
            return user;
        }
        
        /// <summary>
        /// Clears the Security Context Cookie
        /// </summary>
        public void ClearSecurityContext()
        {
            //System.Web.HttpContext.Current.Response.Cookies.Clear();
            //HttpCookie c = new HttpCookie(ScConstants.AuthContext) {Expires = DateTime.Now.AddDays(-1)};
            //System.Web.HttpContext.Current.Response.Cookies.Add(c);

            string[] myCookies = System.Web.HttpContext.Current.Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                System.Web.HttpContext.Current.Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                
            }
            //System.Web.HttpContext.Current.Request.Cookies.Clear();
            //System.Web.HttpContext.Current.Response.Cookies.Clear();
        }
        /// <summary>
        /// Clears the Security Context Cookie
        /// </summary>
        public void ClearAllCookie()
        {
            string[] myCookies = System.Web.HttpContext.Current.Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                System.Web.HttpContext.Current.Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }

            //System.Web.HttpContext.Current.Response.Cookies.Clear();
            //HttpCookie c = new HttpCookie(ScConstants.AuthContext) { Expires = DateTime.Now.AddDays(-1) };
            //System.Web.HttpContext.Current.Response.Cookies.Add(c);

        }
        /// <summary>
        /// Sets the logged in users Security Context
        /// </summary>
        /// <param name="context"></param>
        public void SetSecurityContext(AuthContext context)
        {
            if (System.Web.HttpContext.Current.Request.Cookies[ScConstants.AuthContext] != null)
            {
                System.Web.HttpContext.Current.Response.Cookies.Clear();
                HttpCookie c = new HttpCookie(ScConstants.AuthContext) { Expires = DateTime.Now.AddDays(-1) };
                System.Web.HttpContext.Current.Response.Cookies.Add(c);
            }
            var outputData = ObjectToBase64(context);
            var textToOutPut = Encoding.UTF8.GetBytes(outputData);
            var encryptedValue = Convert.ToBase64String(MachineKey.Protect(textToOutPut, ScConstants.AuthContextPurpose));

            HttpCookie cookieObject = new HttpCookie(ScConstants.AuthContext, encryptedValue);
            //Makue sure it can't be accessed at client side.
            cookieObject.HttpOnly = true;
            System.Web.HttpContext.Current.Response.Cookies.Add(cookieObject);
        }



        /// <summary>
        /// Updates the logged in users Security Context
        /// </summary>
        /// <param name="context"></param>
        public AuthContext UpdateSecurityContext(string context)
        {

            if (System.Web.HttpContext.Current.Request.Cookies[ScConstants.AuthContext] != null)
            {
                System.Web.HttpContext.Current.Response.Cookies.Clear();
                HttpCookie c = new HttpCookie(ScConstants.AuthContext) { Expires = DateTime.Now.AddDays(-1) };
                System.Web.HttpContext.Current.Response.Cookies.Add(c);
            }

            RestSharp.IRestResponse response = GetRefreshAuthToken(context);
            if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Unauthorized)
                return null;
            else
            {
                string content = response.Content.Replace("as:", string.Empty);
                JavaScriptSerializer jsonserializer = new JavaScriptSerializer();
                AuthContext user = jsonserializer.Deserialize<AuthContext>(content);
                if (user.Access_Token == null)
                    return null;
                user.LoggedInUser = context;
                var outputData = ObjectToBase64(user);
                var textToOutPut = Encoding.UTF8.GetBytes(outputData);
                var encryptedValue = Convert.ToBase64String(MachineKey.Protect(textToOutPut, ScConstants.AuthContextPurpose));

                HttpCookie cookieObject = new HttpCookie(ScConstants.AuthContext, encryptedValue);
                cookieObject.HttpOnly = true;
                System.Web.HttpContext.Current.Response.Cookies.Add(cookieObject);
                return user;
            }
        }
        /// <summary>
        /// Sets the logged in users Security Context
        /// </summary>
        /// <param name="context"></param>
        public void SetAuthenticatedUserContext(UserContextModel context)
        {
            var outputData = ObjectToBase64(context);
            var textToOutPut = Encoding.UTF8.GetBytes(outputData);
            var encryptedValue = Convert.ToBase64String(MachineKey.Protect(textToOutPut, ScConstants.AuthContextPurpose));

            HttpCookie cookieObject = new HttpCookie(ScConstants.AuthUserContext, encryptedValue);
            //Makue sure it can't be accessed at client side.
            cookieObject.HttpOnly = true;
            System.Web.HttpContext.Current.Response.Cookies.Add(cookieObject);
        }
        /// <summary>
        /// To get the logged in users Security Context
        /// </summary>
        /// <returns></returns>
        public AuthContext GetSecurityContext()
        {
            AuthContext context = null;
            if (System.Web.HttpContext.Current.Request.Cookies[ScConstants.AuthContext] != null)
            {
                var bytes = Convert.FromBase64String(System.Web.HttpContext.Current.Request.Cookies[ScConstants.AuthContext].Value);
                byte[] output = MachineKey.Unprotect(bytes, ScConstants.AuthContextPurpose);
                string originalData = Encoding.UTF8.GetString(output);
                context = Base64ToObject(originalData) as AuthContext;
            }
            return context;
        }
        /// <summary>
        /// To get the logged in users Context
        /// </summary>
        /// <returns></returns>
        public UserContextModel GetUserContext()
        {
            UserContextModel context = null;
            if (System.Web.HttpContext.Current.Request.Cookies[ScConstants.AuthUserContext] != null)
            {
                var bytes = Convert.FromBase64String(System.Web.HttpContext.Current.Request.Cookies[ScConstants.AuthUserContext].Value);
                byte[] output = MachineKey.Unprotect(bytes, ScConstants.AuthContextPurpose);
                string originalData = Encoding.UTF8.GetString(output);
                context = Base64ToObject(originalData) as UserContextModel;
            }
            return context;
        }
        private string ObjectToBase64(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, obj);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        private object Base64ToObject(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return new BinaryFormatter().Deserialize(ms);
            }
        }
    }
}
