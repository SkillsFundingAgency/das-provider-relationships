using SFA.DAS.Validation.Mvc;
using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.AddValidationFilter();

            filters.Add(new AccountLinksFilter(() => (DependencyResolver.Current.GetService<ProviderRelationshipsConfiguration>(), DependencyResolver.Current.GetService<IAuthenticationUrls>()) ));
        }
    }
}
