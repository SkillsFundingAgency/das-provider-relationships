using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Urls;

namespace SFA.DAS.ProviderRelationships.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IEmployerUrls Urls(this HtmlHelper htmlHelper)
        {
            return (IEmployerUrls)htmlHelper.ViewBag.Urls;
        }
    }
}