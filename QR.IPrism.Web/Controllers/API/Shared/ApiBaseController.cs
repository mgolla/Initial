using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web;
using QR.IPrism.Security.Authentication;
using QR.IPrism.Models.Shared;
using QR.IPrism.Web.Common;
using System.Web.Http.Controllers;

namespace QR.IPrism.Web.API.Shared
{
    [ValidateToken]
    public class ApiBaseController : ApiController
    {
        ISecurityManager _serviceManager = null;
        public ApiBaseController()
        {
            _serviceManager = new SecurityManager();
        }
        public string LoggedInStaffNo
        {
            get
            {
                return UserContext.StaffNumber;
            }
        }
        public string LoggedInStaffDetailId
        {
            get
            {
                return UserContext.CrewDetailsId;
            }
        }
        public string LoggedInUserId
        {
            get
            {
                return UserContext.UserId;
            }
        }
        public UserContextModel UserContext
        {
            get
            {
                return _serviceManager.GetLoggedinUserContext();
            }
        }
        //public override void OnAuthorization(HttpActionContext actionContext)
        //{
        //    //string username;
        //    //string password;
        //    //if (GetUserNameAndPassword(actionContext, out username, out password))
        //    //{
        //    //    if (Membership.ValidateUser(username, password))
        //    //    {
        //    //        if (!isUserAuthorized(username))
        //    //            actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
        //    //    }
        //    //    else
        //    //    {
        //    //        actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
        //    //}
        //}
    }
}
