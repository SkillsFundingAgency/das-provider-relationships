using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;
using SFA.DAS.ProviderRelationships.Api.Client.TestHarness.DependencyResolution;
using SFA.DAS.ProviderRelationships.Api.Client.TestHarness.Scenarios;

namespace SFA.DAS.ProviderRelationships.Api.Client.TestHarness
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Provider Relationships Api Client Test Harness");

            Task.Run(Test).Wait();
        }

        private static async Task Test()
        {
            var configuration = new ProviderRelationshipsApiConfiguration {
                ApiBaseUrl = "https://das-at-prelapi-as.azurewebsites.net/",
                //ApiBaseUrl = "https://localhost:44308/",
                IdentifierUri = "https://citizenazuresfabisgov.onmicrosoft.com/das-at-prelapi-as-ar"
            };


            using (var serviceProvider = IoC.InitializeServiceProvider(configuration))
            {
                try
                {
                    await serviceProvider.GetRequiredService<PingScenario>().Run();
                    await serviceProvider.GetRequiredService<GetAccountProviderLegalEntitiesWithPermissionScenario>().Run();
                    await serviceProvider.GetRequiredService<HasPermissionScenario>().Run();
                    await serviceProvider.GetRequiredService<HasRelationshipWithPermissionScenario>().Run();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            
            using (var container = IoC.InitializeContainer(configuration))
            {
                try
                {   
                    await container.GetInstance<PingScenario>().Run();
                    await container.GetInstance<GetAccountProviderLegalEntitiesWithPermissionScenario>().Run();
                    await container.GetInstance<HasPermissionScenario>().Run();
                    await container.GetInstance<HasRelationshipWithPermissionScenario>().Run();
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