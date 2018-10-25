using System.Web.Mvc;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.ProviderRelationships.Authentication.Mvc;
using SFA.DAS.UnitOfWork.Mvc;
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
            filters.Add(DependencyResolver.Current.GetService<AccountUrlsViewBagFilter>());
        }
    }
}