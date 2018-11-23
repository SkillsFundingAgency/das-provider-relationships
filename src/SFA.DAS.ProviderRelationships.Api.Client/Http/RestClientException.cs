using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Api.Client.Http
{
    public class RestClientException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string ReasonPhrase { get; }
        public Uri RequestUri { get; }
        public string ErrorResponse { get; }
        
//        public RestClientException(HttpStatusCode statusCode, string reasonPhrase, Uri requestUri, string errorResponse)
//        {
//            StatusCode = statusCode;
//            ReasonPhrase = reasonPhrase;
//            RequestUri = requestUri;
//            ErrorResponse = errorResponse;
//        }

        //todo: add proper serialization support?

        public RestClientException(HttpResponseMessage httpResponseMessage, string errorResponse)
            : base(GenerateMessage(httpResponseMessage, errorResponse))
        {
            StatusCode = httpResponseMessage.StatusCode;
            ReasonPhrase = httpResponseMessage.ReasonPhrase;
            RequestUri = httpResponseMessage.RequestMessage.RequestUri;
            ErrorResponse = errorResponse;
        }
        
        // assumes response content hasn't already been read
        public static async Task<RestClientException> Create(HttpResponseMessage httpResponseMessage)
        {
            return new RestClientException(httpResponseMessage, await httpResponseMessage.Content.ReadAsStringAsync());
        }

        private static string GenerateMessage(HttpResponseMessage httpResponseMessage, string errorResponse)
        {
            return
$@"Request '{httpResponseMessage.RequestMessage.RequestUri}' returned {httpResponseMessage.StatusCode} {httpResponseMessage.ReasonPhrase}
Response:
{errorResponse}";
        }
    }

}