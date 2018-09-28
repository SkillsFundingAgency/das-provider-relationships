using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels
{
    public class LayoutViewModel
    {
        private static LayoutViewModel _instance;
        public static LayoutViewModel Instance => _instance ??
            (_instance = DependencyResolver.Current.GetService<LayoutViewModel>());

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
    }
}