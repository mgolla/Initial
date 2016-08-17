using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Models.Shared;

namespace QR.IPrism.Security.Authentication
{
    public interface ISecurityManager
    {
        UserContextModel GetLoggedinUserContext();
        bool UpdateUserContextCaptcha(bool isCatpchaFilled);
        bool SetLoggedinUserContext(string userName);
        string SetImpersoneteUserContext(string userName);
        void SetSecurityContext(AuthContext context);
        bool SetUserContext(string userName);
        void SetAuthenticatedUserContext(UserContextModel context);
        void ClearSecurityContext();
        AuthContext GetSecurityContext();
        UserContextModel GetUserContext();
        AuthContext UpdateSecurityContext(string context);
        AuthContext SetImpUserContext(string userName);
        AuthContext SetContext(string content);
        void SetCookie(string context, string coockieName);
        string GetCookie(string coockieName);
    }
}
