using System.Web.Mvc;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.ProviderRelationships.Web.Filters;
using SFA.DAS.ProviderRelationships.Web.Urls;
using SFA.DAS.UnitOfWork.Mvc.Extensions;
using SFA.DAS.Validation.Mvc;

namespace SFA.DAS.ProviderRelationships.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.AddAuthorizationFilter();
            filters.AddUnauthorizedAccessExceptionFilter();
            filters.AddUnitOfWorkFilter();
            filters.AddValidationFilter();
            filters.Add(new UrlsViewBagFilter(() => DependencyResolver.Current.GetService<IEmployerUrls>()));
            filters.Add(new GoogleAnalyticsFilter());
        }
    }    
}