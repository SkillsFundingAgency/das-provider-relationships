using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.Types;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Task.Run(CallIt).Wait();

            Console.WriteLine("Press ENTER to finish");
            Console.ReadLine();

        }

        static async Task CallIt()
        {

            var ioc = IoC.Initialize();

            //var rep = ioc.GetInstance<IReadOnlyDocumentRepository<ProviderPermissions>>();
            var apiClient = ioc.GetInstance<IProviderRelationshipsApiClient>();
            var result = await apiClient.HasRelationshipWithPermission(new ProviderRelationshipsRequest { Ukprn = 100025, Operation = Operation.CreateCohort });

            if (result == true)
            {
                Console.WriteLine("Yes");
            }

            var rep = ioc.GetInstance<IReadOnlyDocumentRepository<ProviderPermission>>();
            try
            {

                var q = rep.CreateQuery();
                var wrapper = q.Where(m => m.Ukprn == 100024 && m.Metadata.SchemaType == "ProviderPermission").AsDocumentQueryWrapper();
                var docs = await wrapper.ExecuteNextAsync(CancellationToken.None);
                docs = docs.Where(m => m.Operations != null && m.Operations.Any(x => x == Operation.CreateCohort));
                var items = docs.ToList();


                //var q = rep.CreateQuery();
                //var docs = await rep.ExecuteQuery(q.Where(m => m.Ukprn == 100024 && m.Metadata.SchemaType == "ProviderPermission"), CancellationToken.None);
                //docs = docs.Where(m=>m.GrantPermissions != null && m.GrantPermissions.Any(x=>x?.Permission == Operation.CreateCohort));
                //var items = docs.ToList();

                Console.WriteLine($"Count {items.Count}");

                foreach (var item in items)
                {
                    Console.WriteLine(
                        $"UKPRN : {item.Ukprn} EmployerAccountId {item.EmployerAccountId} and ID : {item.Id} and ETag @ {item.ETag} ");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }


    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.AddRegistry<ProviderRelationshipsApiClientRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }

    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {

            For<IDocumentConfiguration>().Use(new CosmosDbConfiguration
            {
                DatabaseName = "SFA",
                Uri = "https://localhost:8081",
                SecurityKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                MaxRetryAttemptsOnThrottledRequests = 3,
                MaxRetryWaitTimeInSeconds = 2
            });
            For(typeof(IDocumentRepository<>)).Use(typeof(DocumentRepository<>)).Ctor<string>()
                .Is("provider-relationships");
            For(typeof(IReadOnlyDocumentRepository<>)).Use(typeof(ReadOnlyDocumentRepository<>)).Ctor<string>()
                .Is("provider-relationships");

        }
    }


}
