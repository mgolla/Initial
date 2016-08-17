using System;
using System.Net.Http;
using System.Web;
using System.Web.Http.ExceptionHandling;
using Elmah;

namespace QR.IPrism.Exception.ExceptionManagement
{
    public class ExceptionLog : ExceptionLogger
    {
        private const string HttpContextBaseKey = "MS_OwinContext";
        public ExceptionLog() { }

        public override void Log(ExceptionLoggerContext context)
           {
            string errorId = Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(context.Exception,HttpContext.Current));
            HttpContext.Current.Request.RequestContext.RouteData.Values["CorrelationID"] = errorId;
        }

        public static HttpContext GetHttpContext(HttpRequestMessage request)
        {
            HttpContextBase contextBase = GetHttpContextBase(request);

            if (contextBase == null)
            {
                return null;
            }

            return ToHttpContext(contextBase);
        }

        public static HttpContextBase GetHttpContextBase(HttpRequestMessage request)
        {
            if (request == null)
            {
                return null;
            }

            object value;

            if (!request.Properties.TryGetValue(HttpContextBaseKey, out value))
            {
                return null;
            }

            return value as HttpContextBase;
        }

        private static HttpContext ToHttpContext(HttpContextBase contextBase)
        {
            return contextBase.ApplicationInstance.Context;
        }
    }
}
