using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Urls;

namespace SFA.DAS.ProviderRelationships.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IViewUrls Urls(this HtmlHelper htmlHelper)
        {
            return (IViewUrls) htmlHelper.ViewBag.Urls;
        }
    }
}