using System;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Api.Client.TestHarness.DependencyResolution;
using SFA.DAS.ProviderRelationships.Api.Client.TestHarness.Scenarios;

namespace SFA.DAS.ProviderRelationships.Api.Client.TestHarness
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Provider Relationships Api Client TestHarness");

            Task.Run(Test).Wait();
        }

        private static async Task Test()
        {
            using (var container = IoC.Initialize())
            {
                try
                {
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