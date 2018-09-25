using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Web.App_Start;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels
{
    public class LayoutViewModel
    {
        private static LayoutViewModel _instance;
        public static LayoutViewModel Instance => _instance ??
            (_instance = StructuremapMvc.StructureMapDependencyScope.Container.GetInstance<LayoutViewModel>());

        public string ChangePasswordLink { get; }
        public string ChangeEmailLink { get; }

        public LayoutViewModel(
            ProviderRelationshipsConfiguration providerRelationshipsConfig,
            IAuthenticationUrls authenticationUrls)
        {
            var urlHelper = new UrlHelper();
            // the second interpolated expression is the return url (we send them back to MA)
            ChangePasswordLink = $"{authenticationUrls.ChangePasswordLink}{urlHelper.Encode(providerRelationshipsConfig.EmployerPortalBaseUrl.TrimEnd('/') + "/service/password/change")}";
            ChangeEmailLink = $"{authenticationUrls.ChangeEmailLink}{urlHelper.Encode(providerRelationshipsConfig.EmployerPortalBaseUrl.TrimEnd('/') + "/service/email/change")}";
        }

        //private string GenerateChangeUrl(UrlHelper urlHelper, ProviderRelationshipsConfiguration providerRelationshipsConfig, string urlStart, string returnUrlEnd)
        //{
        //    //todo: create Uri with scheme etc. instead of hardcoding
        //    return $"{urlStart}{urlHelper.Encode("https://" + providerRelationshipsConfig.EmployerPortalBaseUrl + returnUrlEnd)}";
        //}
    }
}