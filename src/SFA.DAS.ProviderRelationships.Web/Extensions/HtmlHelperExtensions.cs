using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.App_Start;
using SFA.DAS.ProviderRelationships.Web.Urls;

namespace SFA.DAS.ProviderRelationships.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IEmployerUrls EmployerUrls(this HtmlHelper htmlHelper)
        {
            return (IEmployerUrls)htmlHelper.ViewBag.EmployerUrls;
        }

        public static MvcHtmlString CdnLink(this HtmlHelper html, string folderName, string fileName)
        {
            var cdnLocation = StructuremapMvc.StructureMapDependencyScope.Container.GetInstance<ProviderRelationshipsConfiguration>().CdnBaseUrl;

            var trimCharacters = new char[] { '/' };
            return new MvcHtmlString($"{cdnLocation.Trim(trimCharacters)}/{folderName.Trim(trimCharacters)}/{fileName.Trim(trimCharacters)}");
        }
    }
}