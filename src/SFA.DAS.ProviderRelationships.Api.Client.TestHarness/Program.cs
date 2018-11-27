using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.AutoConfiguration;
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

        private const string VariablePrefix = "AppSettings_";

        private static void SetupEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable(VariablePrefix + EnvironmentVariableNames.Environment, "LOCAL");
            Environment.SetEnvironmentVariable(VariablePrefix + EnvironmentVariableNames.ConfigurationStorageConnectionString, "UseDevelopmentStorage=true");
        }

        private static async Task Test()
        {
            using (var container = IoC.Initialize())
            {
                try
                {
                    SetupEnvironmentVariables();
                    var relationshipsRepository = container.GetInstance<IRelationshipsRepository>();

                    var relationship = new Relationship(
                        accountId: 1,
                        accountLegalEntityId: 3,
                        accountProviderId: 4,
                        accountProviderLegalEntityId: 34,
                        ukprn: 12345678,
                        grantedOperations: new HashSet<Operation>(),
                        created: DateTime.UtcNow.AddDays(-1),
                        messageId: "ede68da8-4874-414f-b5fd-1fe31cb0b8dc");

                    await relationshipsRepository.Add(relationship);

                    relationship.UpdatePermissions(new HashSet<Operation> { Operation.CreateCohort }, DateTime.UtcNow, "5135744e-4247-474f-8b6a-602b1702e32c");

                    await relationshipsRepository.Update(relationship);

                    var apiClient = container.GetInstance<IProviderRelationshipsApiClient>();
                    var relationshipsRequest = new RelationshipsRequest { Ukprn = relationship.Ukprn, Operation = Operation.CreateCohort };
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