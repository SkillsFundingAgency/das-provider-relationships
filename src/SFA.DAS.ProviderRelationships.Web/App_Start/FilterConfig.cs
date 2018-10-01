using SFA.DAS.Validation.Mvc;
using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Authentication;

namespace SFA.DAS.ProviderRelationships.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.AddValidationFilter();
            filters.Add(DependencyResolver.Current.GetService<AccountLinksInViewBagFilter>());
        }
    }
}
