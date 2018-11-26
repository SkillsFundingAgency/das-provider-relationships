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

        //todo: add these to das-employer-config with correct values
        //https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-v1-dotnet-webapi
        //ida:Audience is the App ID URI of the application that you entered in the Azure portal.
        
        public string idaAudience { get; set; }
        public string idaTenant { get; set; }
        
        public string NServiceBusLicense
        {
            get =>  _decodedNServiceBusLicense ?? (_decodedNServiceBusLicense = _nServiceBusLicense.HtmlDecode());
            set => _nServiceBusLicense = value;
        }
        
        private string _nServiceBusLicense;
        private string _decodedNServiceBusLicense;
    }
}