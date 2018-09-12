using System.Web;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.ProviderRelationships.Web.Logging
{
    public sealed class LoggingContext : ILoggingContext
    {
        public string HttpMethod { get; }
        public bool? IsAuthenticated { get; }
        public string Url { get; }
        public string UrlReferrer { get; }
        public string ServerMachineName { get; }

        public LoggingContext(HttpContextBase context)
        {
            HttpMethod = context?.Request.HttpMethod;
            IsAuthenticated = context?.Request.IsAuthenticated;
            Url = context?.Request.Url?.PathAndQuery;
            UrlReferrer = context?.Request.UrlReferrer?.PathAndQuery;
            ServerMachineName = context?.Server.MachineName;
        }
    }
}