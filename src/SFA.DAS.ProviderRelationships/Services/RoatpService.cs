using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Http;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Services
{
    public class RoatpService : IRoatpService
    {
        private IRestHttpClient _client;

        public RoatpService(IRoatpApiHttpClientFactory roatpApiHttpClientFactory)
        {
            _client = roatpApiHttpClientFactory.CreateRestHttpClient();
        }

        public async Task<bool> Ping()
        {
            try
            {
                await _client.Get("ping");
            }
            catch (RestHttpClientException )
            {
                return false;
            }
            
            return true;
        }

        public async Task<IEnumerable<ProviderRegistration>> GetProviders()
        {
            var providers = await _client.Get<List<ProviderRegistration>>("v1/fat-data-export");

            return providers;
        }
    }
}