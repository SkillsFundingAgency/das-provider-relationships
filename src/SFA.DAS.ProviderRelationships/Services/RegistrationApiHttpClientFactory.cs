using SFA.DAS.Http;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Services
{
    public class RegistrationApiHttpClientFactory : IRegistrationApiHttpClientFactory
    {
        private readonly RegistrationApiConfiguration _registrationApiConfiguration;

        public RegistrationApiHttpClientFactory(RegistrationApiConfiguration registrationApiConfiguration)
        {
            _registrationApiConfiguration = registrationApiConfiguration;
        }

        public IRestHttpClient CreateRestHttpClient()
        {
            return new RestHttpClient(new AzureActiveDirectoryHttpClientFactory(_registrationApiConfiguration).CreateHttpClient());
        }
    }
}
