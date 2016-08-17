using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QR.IPrism.Logging
{
    public class HttpContextProvider
    {
        public override string ToString()
        {
            HttpContext context = HttpContext.Current;
            if (context != null && context.User != null && context.User.Identity.IsAuthenticated)
            {
                return context.User.Identity.Name;
            }
            return "";
        }
    }
}
