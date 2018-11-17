using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Api.Client.TestHarness.DependencyResolution;
using SFA.DAS.ProviderRelationships.Document.Repository;
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
            Console.WriteLine("Hello World!");

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
                    var relationshipsRepository = container.GetInstance<IRelationshipsRepository>();

                    var relationship = new Relationship(
                        ukprn: 2001877,
                        accountProviderLegalEntityId: 2123,
                        accountId: 11222,
                        accountPublicHashedId: "HASHED11123",
                        accountName: "AccountNameBB",
                        accountLegalEntityId: 113333,
                        accountLegalEntityPublicHashedId: "HASH11333",
                        accountLegalEntityName: "LENameAGHY",
                        accountProviderId: 111234,
                        created: DateTime.UtcNow.AddDays(-1),
                        messageId: "85234231-4975-4ded-a167-a996009eb90e");

                    await relationshipsRepository.Add(relationship);

                    relationship.UpdatePermissions(new HashSet<Operation> { Operation.CreateCohort }, DateTime.UtcNow, "0d901e4f-05ef-4ebc-82d4-a99600a27f55");

                    await relationshipsRepository.Update(relationship);

                    var apiClient = container.GetInstance<IProviderRelationshipsApiClient>();
                    var relationshipsRequest = new RelationshipsRequest { Ukprn = relationship.AccountProvider.Ukprn, Operation = Operation.CreateCohort };
                    var response = await apiClient.GetRelationshipsWithPermission(relationshipsRequest);

                    if (response.Relationships.Any())
                    {
                        Console.WriteLine($"Provider with UKPRN {relationshipsRequest.Ukprn} has relationship with {relationshipsRequest.Operation} permission");
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