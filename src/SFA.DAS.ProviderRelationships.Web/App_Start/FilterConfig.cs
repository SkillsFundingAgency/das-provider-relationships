using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.UnitOfWork.Mvc;
using SFA.DAS.Validation.Mvc;

namespace SFA.DAS.ProviderRelationships.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.AddUnitOfWorkFilter();
            filters.AddValidationFilter();
            filters.Add(DependencyResolver.Current.GetService<AccountLinksInViewBagFilter>());
        }
    }
}
