using System;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Api.Client.TestHarness.Scenarios
{
    public class PingScenario
    {
        private readonly IProviderRelationshipsApiClient _providerRelationshipsApiClient;

        public PingScenario(IProviderRelationshipsApiClient providerRelationshipsApiClient)
        {
            _providerRelationshipsApiClient = providerRelationshipsApiClient;
        }

        public async Task Run()
        {
            await _providerRelationshipsApiClient.Ping();

            Console.WriteLine("Ping succeeded");
        }
    }
}