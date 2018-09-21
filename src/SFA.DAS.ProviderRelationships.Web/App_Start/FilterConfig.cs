using SFA.DAS.Validation.Mvc;
using System.Web.Mvc;

namespace SFA.DAS.ProviderRelationships.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.AddValidationFilter();
        }
    }
}
