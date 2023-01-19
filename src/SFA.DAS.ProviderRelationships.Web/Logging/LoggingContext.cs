using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;


namespace SFA.DAS.ProviderRelationships.Web.Logging
{
    /*public sealed class LoggingContext : ILoggingContext
    {
        public string HttpMethod { get; }
        public bool? IsAuthenticated { get; }
        public string Url { get; }
        public string UrlReferrer { get; }
        public string ServerMachineName { get; }

        public LoggingContext(HttpContext context)
        {
            HttpMethod = context?.Request.Method;
            //IsAuthenticated = context?.Request.Headers.Authorization.;
            Url = context?.Request.GetDisplayUrl();
            ServerMachineName = context?.Connection.LocalIpAddress.ToString();
        }
    }*/
}