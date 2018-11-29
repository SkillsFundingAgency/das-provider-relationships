using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace SFA.DAS.ProviderRelationships.Api.HttpErrorResult
{
    public class ErrorResult : IHttpActionResult
    {
        internal readonly HttpStatusCode HttpStatusCode;
        internal readonly Error Error;
        internal readonly HttpRequestMessage Request;

        public ErrorResult(HttpStatusCode httpStatusCode, Error error, HttpRequestMessage request)
        {
            HttpStatusCode = httpStatusCode;
            Error = error;
            Request = request;
        }
        
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode)
            {
                Content = new ObjectContent<Error>(Error, new JsonMediaTypeFormatter()),
                RequestMessage = Request
            });
        }
    }
}