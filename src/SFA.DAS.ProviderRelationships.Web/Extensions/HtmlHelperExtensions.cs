using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Web.Urls;

namespace SFA.DAS.ProviderRelationships.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IEmployerUrls EmployerUrls(this HtmlHelper htmlHelper)
        {
            return (IEmployerUrls)htmlHelper.ViewBag.EmployerUrls;
        }
    }
}