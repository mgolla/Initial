using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using QR.IPrism.Exception.ExceptionManagement;

namespace QR.IPrism.Exception.Results
{
    public class InternalServerErrorTextPlainResult : IHttpActionResult
    {
        public InternalServerErrorTextPlainResult(string content, Encoding encoding, HttpRequestMessage request)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            Content = content;
            Encoding = encoding;
            Request = request;
        }

        public string Content { get; private set; }

        public Encoding Encoding { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError
                    , GenerateErrorResponse());
        }

        public ErrorResponse GenerateErrorResponse()
        {
            return new ErrorResponse { ErrorCode = string.Empty, CorelationId = HttpContext.Current.Request.RequestContext.RouteData.Values["CorrelationID"].ToString().ToUpper().Replace("-",""), ErrorMessage = Content };
        }
    }
}