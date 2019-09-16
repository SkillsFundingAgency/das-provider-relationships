using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
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
            using (var serviceProvider = IoC.InitializeServiceProvider())
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
            
            using (var container = IoC.InitializeContainer())
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