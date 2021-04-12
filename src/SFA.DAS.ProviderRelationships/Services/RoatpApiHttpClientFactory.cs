using SFA.DAS.Http;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Services
{
    public class RoatpApiHttpClientFactory : IRoatpApiHttpClientFactory
    {
        private readonly RoatpApiConfiguration _roatpApiConfiguration;

        public RoatpApiHttpClientFactory (RoatpApiConfiguration roatpApiConfiguration)
        {
            _roatpApiConfiguration = roatpApiConfiguration;
        }
        public IRestHttpClient CreateRestHttpClient()
        {
            return new RestHttpClient(new ManagedIdentityHttpClientFactory(_roatpApiConfiguration).CreateHttpClient());
        }
    }
    
}