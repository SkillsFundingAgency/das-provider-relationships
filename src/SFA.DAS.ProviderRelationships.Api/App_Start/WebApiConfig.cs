using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using ExceptionLogger = SFA.DAS.ProviderRelationships.Api.ExceptionLoggers.ExceptionLogger;

namespace SFA.DAS.ProviderRelationships.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Services.Add(typeof(IExceptionLogger), new ExceptionLogger());
        }
    }
}
