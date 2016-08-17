using System.Configuration;
using System.Text;
using System.Web.Http.ExceptionHandling;
using QR.IPrism.Exception.Results;
using Elmah;

namespace QR.IPrism.Exception.ExceptionManagement
{
    public class GenericTextExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new InternalServerErrorTextPlainResult(
                ConfigurationManager.AppSettings["GlobalErrorMsg"].ToString(),
                Encoding.UTF8, context.Request);
        }
        public override bool ShouldHandle(ExceptionHandlerContext context)
        {
            return true;
        }
    }
}