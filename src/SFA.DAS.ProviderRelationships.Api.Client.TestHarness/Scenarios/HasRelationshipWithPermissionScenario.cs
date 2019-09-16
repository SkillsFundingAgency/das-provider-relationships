using System;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.TestHarness.Scenarios
{
    public class HasRelationshipWithPermissionScenario
    {
        private readonly IProviderRelationshipsApiClient _providerRelationshipsApiClient;
        
        public HasRelationshipWithPermissionScenario(IProviderRelationshipsApiClient providerRelationshipsApiClient)
        {
            _providerRelationshipsApiClient = providerRelationshipsApiClient;
        }
        
        public async Task Run()
        {
            const long ukprn = 10005077;

            var hasRelationshipWithPermissionRequest = new HasRelationshipWithPermissionRequest { Ukprn = ukprn, Operation = Operation.CreateCohort };
            var hasRelationshipWithPermission = await _providerRelationshipsApiClient.HasRelationshipWithPermission(hasRelationshipWithPermissionRequest);

            Console.WriteLine($"Calling HasRelationshipWithPermission with Ukprn {hasRelationshipWithPermissionRequest.Ukprn}, Operation {hasRelationshipWithPermissionRequest.Operation} returned {hasRelationshipWithPermission}");
        }
    }
}