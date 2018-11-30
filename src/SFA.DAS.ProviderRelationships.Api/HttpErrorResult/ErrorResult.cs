//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Formatting;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web.Http;
//using SFA.DAS.ProviderRelationships.Types.Errors;
//
//namespace SFA.DAS.ProviderRelationships.Api.HttpErrorResult
//{
//    public class ErrorResult : IHttpActionResult
//    {
//        internal readonly HttpStatusCode HttpStatusCode;
//        internal readonly ErrorResponse ErrorResponse;
//        internal readonly HttpRequestMessage Request;
//
//        public ErrorResult(HttpStatusCode httpStatusCode, ErrorResponse errorResponse, HttpRequestMessage request)
//        {
//            HttpStatusCode = httpStatusCode;
//            ErrorResponse = errorResponse;
//            Request = request;
//        }
//        
//        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
//        {
//            return Task.FromResult(new HttpResponseMessage(HttpStatusCode)
//            {
//                Content = new ObjectContent<ErrorResponse>(ErrorResponse, new JsonMediaTypeFormatter()),
//                RequestMessage = Request
//            });
//        }
//    }
//}