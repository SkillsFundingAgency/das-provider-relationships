using System.Net;
using System.Net.Http;
using SFA.DAS.ProviderRelationships.Types.Errors;

namespace SFA.DAS.ProviderRelationships.Api.HttpErrorResult
{
    public static class HttpRequestMessageExtensions
    {
        public static ErrorResult ErrorResult(this HttpRequestMessage request, HttpStatusCode httpStatusCode, string errorCode, string message)
        {
            return new ErrorResult(httpStatusCode, new ErrorResponse(errorCode, message), request);
        }
    }
}