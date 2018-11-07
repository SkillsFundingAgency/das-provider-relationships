using System.Web.Mvc;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    public class AccountUrls
    {
        public string ChangePasswordUrl { get; }
        public string ChangeEmailUrl { get; }

        public AccountUrls(ProviderRelationshipsConfiguration providerRelationshipsConfig, IAuthenticationUrls authenticationUrls)
        {
            var urlHelper = new UrlHelper();
            // the second interpolated expression is the return url (we send them back to MA)
            ChangePasswordUrl = $"{authenticationUrls.ChangePasswordUrl}{urlHelper.Encode($"{providerRelationshipsConfig.EmployerPortalBaseUrl.TrimEnd('/')}/service/password/change")}";
            ChangeEmailUrl = $"{authenticationUrls.ChangeEmailUrl}{urlHelper.Encode($"{providerRelationshipsConfig.EmployerPortalBaseUrl.TrimEnd('/')}/service/email/change")}";
        }
    }
}
