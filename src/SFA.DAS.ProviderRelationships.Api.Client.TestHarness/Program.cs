using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Api.Client.TestHarness.DependencyResolution;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.Types.Dtos;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Api.Client.TestHarness
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Provider Relationships Api Client TestHarness");

            Task.Run(Test).Wait();

            Console.WriteLine("Press ENTER to finish");
            Console.ReadLine();
        }

        private static async Task Test()
        {
            using (var container = IoC.Initialize())
            {
                try
                {
                    var accountProviderLegalEntitiesRepository = container.GetInstance<IAccountProviderLegalEntitiesRepository>();

                    var accountProviderLegalEntity = new AccountProviderLegalEntity(
                        accountId: 1,
                        accountLegalEntityId: 3,
                        accountProviderId: 4,
                        accountProviderLegalEntityId: 34,
                        ukprn: 12345678,
                        grantedOperations: new HashSet<Operation>(),
                        created: DateTime.UtcNow.AddDays(-1),
                        messageId: "ede68da8-4874-414f-b5fd-1fe31cb0b8dc");

                    await accountProviderLegalEntitiesRepository.Add(accountProviderLegalEntity);

                    accountProviderLegalEntity.UpdatePermissions(new HashSet<Operation> { Operation.CreateCohort }, DateTime.UtcNow, "5135744e-4247-474f-8b6a-602b1702e32c");

                    await accountProviderLegalEntitiesRepository.Update(accountProviderLegalEntity);

                    var apiClient = container.GetInstance<IProviderRelationshipsApiClient>();
                    var relationshipsRequest = new AccountProviderLegalEntitiesRequest { Ukprn = accountProviderLegalEntity.Ukprn, Operation = Operation.CreateCohort };
                    var response = await apiClient.GetAccountProviderLegalEntitiesWithPermission(relationshipsRequest);

                    if (response.AccountProviderLegalEntities.Any())
                    {
                        Console.WriteLine($"Provider with UKPRN {relationshipsRequest.Ukprn} has AccountProviderLegalEntities with {relationshipsRequest.Operation} permission");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    throw;
                }
            }
        }
    }
}