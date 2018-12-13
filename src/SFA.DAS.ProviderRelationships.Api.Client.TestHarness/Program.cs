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

            Console.WriteLine("Press ENTER to finish");
            Console.ReadLine();
        }

        private static async Task Test()
        {
            using (var container = IoC.Initialize())
            {
                try
                {
                    var getAccountProviderLegalEntitiesWithPermissionScenario =
                        container.GetInstance<GetAccountProviderLegalEntitiesWithPermissionScenario>();

                    await getAccountProviderLegalEntitiesWithPermissionScenario.Run();
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