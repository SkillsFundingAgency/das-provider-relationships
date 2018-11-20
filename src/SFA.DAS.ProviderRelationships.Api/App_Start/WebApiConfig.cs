using System.Web.Http;
using SFA.DAS.UnitOfWork.WebApi;

namespace SFA.DAS.ProviderRelationships.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.AddUnitOfWorkFilter();
            config.MapHttpAttributeRoutes();
        }
    }
}
