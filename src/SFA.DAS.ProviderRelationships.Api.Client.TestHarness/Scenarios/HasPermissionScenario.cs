using System;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.TestHarness.Scenarios
{
    public class HasPermissionScenario
    {
        private readonly IProviderRelationshipsApiClient _providerRelationshipsApiClient;
        
        public HasPermissionScenario(IProviderRelationshipsApiClient providerRelationshipsApiClient)
        {
            _providerRelationshipsApiClient = providerRelationshipsApiClient;
        }
        
        public async Task Run()
        {
            const long ukprn = 10005077;
            const long accountLegalEntityId = 1;

            var hasPermissionRequest = new HasPermissionRequest { Ukprn = ukprn, AccountLegalEntityId = accountLegalEntityId, Operation = Operation.CreateCohort };
            var hasPermission = await _providerRelationshipsApiClient.HasPermission(hasPermissionRequest);

            //we could add a ToString to HasPermissionRequest and use that instead
            Console.WriteLine($"Calling HasPermission with Ukprn {hasPermissionRequest.Ukprn}, AccountLegalEntityId {hasPermissionRequest.AccountLegalEntityId}, Operation {hasPermissionRequest.Operation} returned {hasPermission}");
        }
    }
}