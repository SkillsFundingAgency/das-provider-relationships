using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.ProviderRelationships.Urls;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    public class ProviderRelationshipsConfiguration : IEmployerUrlsConfiguration
    {
        public string AllowedHashstringCharacters { get; set; }
        public string Hashstring { get; set; }
        public string DatabaseConnectionString { get; set; }
        public string EmployerAccountsBaseUrl { get; set; }
        public string EmployerCommitmentsBaseUrl { get; set; }
        public string EmployerFinanceBaseUrl { get; set; }
        public string EmployerPortalBaseUrl { get; set; }
        public string EmployerProjectionsBaseUrl { get; set; }
        public string EmployerRecruitBaseUrl { get; set; }
        public string EmployerUsersBaseUrl { get; set; }
        public string ServiceBusConnectionString { get; set; }
        public IdentityServerConfiguration Identity { get; set; }

        public string Audience { get; set; }
        public string Tenant { get; set; }
        
        public string NServiceBusLicense
        {
            get =>  _decodedNServiceBusLicense ?? (_decodedNServiceBusLicense = _nServiceBusLicense.HtmlDecode());
            set => _nServiceBusLicense = value;
        }
        
        private string _nServiceBusLicense;
        private string _decodedNServiceBusLicense;
    }
}