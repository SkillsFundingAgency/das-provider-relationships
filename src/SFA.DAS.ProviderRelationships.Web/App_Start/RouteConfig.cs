using System.Web.Mvc;
using System.Web.Routing;

namespace SFA.DAS.ProviderRelationships.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapMvcAttributeRoutes();
        }
    }
}
