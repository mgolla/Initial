using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using QR.IPrism.Security.Authentication;

namespace QR.IPrism.Web.Common
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class ValidateTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            try
            {
                ITokenManager tokeManager = new TokenManager();
                IEnumerable<string> verificationTokenHeaders;
                if (actionContext.Request.Headers.TryGetValues("ForgeryVerificationToken", out verificationTokenHeaders))
                {
                    if (!tokeManager.ValidateAntiForgeryToken(verificationTokenHeaders.FirstOrDefault()))
                    {
                        actionContext.Response = new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.Forbidden,
                            RequestMessage = actionContext.ControllerContext.Request
                        };
                        var sourceTask = new TaskCompletionSource<HttpResponseMessage>();
                        sourceTask.SetResult(actionContext.Response);
                        return sourceTask.Task;
                    }
                }
                else
                {
                    actionContext.Response = GenerateUnauthorizedResponse(actionContext);
                    return FromResult(actionContext.Response);
                }

            }
            catch (System.Web.Mvc.HttpAntiForgeryException e)
            {
                actionContext.Response = GenerateUnauthorizedResponse(actionContext);
                return FromResult(actionContext.Response);
            }
            return continuation();
        }
       private HttpResponseMessage GenerateUnauthorizedResponse(HttpActionContext actionContext)
       {
           return new HttpResponseMessage
           {
               StatusCode = HttpStatusCode.Forbidden,
               RequestMessage = actionContext.ControllerContext.Request,
               Content = new StringContent("Unauthorized : Access is denied. You don't have permission to access this resource.")
           };
       }

       private Task<HttpResponseMessage> FromResult(HttpResponseMessage result)
       {
           var source = new TaskCompletionSource<HttpResponseMessage>();
           source.SetResult(result);
           return source.Task;
       }

    }
}