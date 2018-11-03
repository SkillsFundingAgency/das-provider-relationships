using System;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Api.Client.Dtos;
using SFA.DAS.ProviderRelationships.Api.Client.TestHarness.DependencyResolution;
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
                    var apiClient = container.GetInstance<IProviderRelationshipsApiClient>();
                    var relationshipsRequest = new RelationshipsRequest { Ukprn = 11111111, Operation = Operation.CreateCohort };
                    var response = await apiClient.HasRelationshipWithPermission(relationshipsRequest);

                    if (response)
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