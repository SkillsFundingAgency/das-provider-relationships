using System;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.TestHarness.Scenarios
{
    public class GetAccountProviderLegalEntitiesWithPermissionScenario
    {
        private readonly IProviderRelationshipsApiClient _providerRelationshipsApiClient;
        
        public GetAccountProviderLegalEntitiesWithPermissionScenario(IProviderRelationshipsApiClient providerRelationshipsApiClient)
        {
            _providerRelationshipsApiClient = providerRelationshipsApiClient;
        }
        
        public async Task Run()
        {
            const long ukprn = 10005077;

            var relationshipsRequest = new GetAccountProviderLegalEntitiesWithPermissionRequest { Ukprn = ukprn, Operation = Operation.CreateCohort };
            var response = await _providerRelationshipsApiClient.GetAccountProviderLegalEntitiesWithPermission(relationshipsRequest);

            if (response.AccountProviderLegalEntities.Any())
            {
                Console.WriteLine($"Provider with UKPRN {relationshipsRequest.Ukprn} has AccountProviderLegalEntities with {relationshipsRequest.Operation} permission");
            }
        }
    }
}