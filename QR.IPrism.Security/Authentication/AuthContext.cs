using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Models.Shared;
using QR.IPrism.Utility;

namespace QR.IPrism.Security.Authentication
{
    [Serializable]
    public class AuthContext
    {
        public string LoggedInUser { get; set; }
        public string UserId { get; set; }
        public string LoggedInUserDetailsId { get; set; }
        public string ImpersonatedUser { get; set; }
        public string Access_Token { get; set; }
        public string Expires_In { get; set; }
        public string Token_Type { get; set; }
        public string Version { get; set; }
        public string refresh_token { get; set; }
        public string client_id { get; set; }
    }
}
