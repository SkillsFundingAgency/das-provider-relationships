using System;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Document.Model;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb;
using SFA.DAS.ProviderRelationships.Document.Repository.DependencyResolution;
using StructureMap;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Task.Run(CallIt).Wait();

            Console.ReadLine();

        }

        static async Task CallIt()
        {

            var ioc = IoC.Initialize();

            var rep = ioc.GetInstance<IDocumentRepository<ProviderRelationship>>();

            try
            {
                var docs = await rep.Search(m => m.Ukprn == 100024 /*&& m.Permissions.Contains(PermissionEnum.Another)/* && m.EmployerAccountId == 1235 /*&& m.SchemaVersion == 0*/);
                var items = docs.ToList();

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
                c.AddRegistry<DocumentRegistry>();
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
                SecurityKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
            });
            For(typeof(IDocumentRepository<>)).Use(typeof(DocumentRepository<>)).Ctor<string>()
                .Is("provider-relationships");

        }
    }


}
