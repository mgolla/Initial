using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Utility;
using System.Web;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Security;
using System.Configuration;

namespace QR.IPrism.Security.Authentication
{
    public class TokenManager:ITokenManager
    {
        private ISecurityManager manager = null;
        public TokenManager()
        {
            manager = new SecurityManager();
        }
        public string GetAntiForgeryToken()
        {
            string token, time = DateTime.Now.ToString();
            string ipAddress = GetIPAddress();
            string guid = Guid.NewGuid().ToString();

            var context = manager.GetLoggedinUserContext();

            token = ipAddress + "~" + time + "~" + context.CrewDetailsId + "~" + guid + "~" + context.StaffNumber + "~" + ConfigurationManager.AppSettings["isAntyForgeryEnabled"].ToString();

            var outputData = ObjectToBase64(token);
            var textToOutPut = Encoding.UTF8.GetBytes(outputData);
            var encryptedValue = Convert.ToBase64String(MachineKey.Protect(textToOutPut, ScConstants.AuthContextPurpose));
            return encryptedValue;
        }
        public bool ValidateAntiForgeryToken(string token)
        {
            try
            {
                ISecurityManager manager = new SecurityManager();
                var context = manager.GetLoggedinUserContext();
                if (context != null)
                {
                    int timeOutMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOutMinutes"]);

                    var bytes = Convert.FromBase64String(token);
                    byte[] output = MachineKey.Unprotect(bytes, ScConstants.AuthContextPurpose);
                    string originalData = Encoding.UTF8.GetString(output);
                    token = Base64ToObject(originalData).ToString();

                    string ipAddress = GetIPAddress();
                    var tokenList = token.Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries);

                    string tokenIpAddress = tokenList[0];
                    DateTime tokenTime = Convert.ToDateTime(tokenList[1]).AddMinutes(timeOutMinutes);

                    string tokenCrewDetailsId = tokenList[2];
                    string tokenStaffNumber = tokenList[4];
                    bool isAntyForgeryEnabled = Convert.ToBoolean(tokenList[5]);

                    if (isAntyForgeryEnabled)
                    {
                        if (tokenIpAddress.Equals(ipAddress)
                            && tokenStaffNumber.Equals(context.StaffNumber) && tokenCrewDetailsId.Equals(context.CrewDetailsId))
                            return true;
                        else
                            return false;
                    }
                    else
                        return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
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
        private string ObjectToBase64(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, obj);
                return Convert.ToBase64String(ms.ToArray());
            }
        }
        protected string GetIPAddress()
        {
           HttpContext context = HttpContext.Current;
            string ipAddress = string.Empty;
            List<string> keyToCheckList = new List<string>(){"HTTP_X_FORWARDED_FOR",
                                                         "REMOTE_ADDR",
                                                         "HTTP_X_CLUSTER_CLIENT_IP", 
                                                         "HTTP_X_FORWARDED",  
                                                         "HTTP_CLIENT_IP",
                                                         "HTTP_FORWARDED_FOR", 
                                                         "HTTP_FORWARDED", 
                                                         };
            foreach (string key in keyToCheckList)
            {
                if (!string.IsNullOrEmpty(context.Request.ServerVariables[key]))
                {
                    ipAddress = context.Request.ServerVariables[key].ToString();
                    break;
                }
            }
            if (string.IsNullOrEmpty(ipAddress))
                ipAddress = context.Request.UserHostAddress;

            return ipAddress;
        }
    }
}
