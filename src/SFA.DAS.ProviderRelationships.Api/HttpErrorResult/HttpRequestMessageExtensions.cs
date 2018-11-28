using System.Net;
using System.Net.Http;

namespace SFA.DAS.ProviderRelationships.Api.HttpErrorResult
{
    public static class HttpRequestMessageExtensions
    {
        public static ErrorResult ErrorResult(this HttpRequestMessage request, HttpStatusCode httpStatusCode, string errorCode, string message)
        {
            return new ErrorResult(httpStatusCode, new Error(errorCode, message), request);
        }
    }
}