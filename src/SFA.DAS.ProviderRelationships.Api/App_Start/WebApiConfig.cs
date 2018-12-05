using System.Web.Http;

namespace SFA.DAS.ProviderRelationships.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
        }
    }
}
