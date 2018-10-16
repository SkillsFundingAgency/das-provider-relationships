using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb;

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

        static async Task<List<Test>> CallIt()
        {
            var cng = new CosmosDbConfiguration
            {
                DatabaseName = "SFA",
                Uri = "https://localhost:8081",
                SecurityKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
            };

            var rep = new DocumentRepository<Test>(new CosmosClientFactory(), cng, "provider-relationships");

            try
            {
                var docs = await rep.Search(m => /*m.Ukprn == 100024 && */m.EmployerAccountId == 1235);
                var items = docs.ToList();

                Console.WriteLine($"Count {items.Count}");

                foreach (var item in items)
                {
                    Console.WriteLine($"UKPRN : {item.Ukprn} EmployerAccountId {item.EmployerAccountId} and ID : {item.Id} and ETag @ {item.ETag} ");
                }

                return items;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }

        public class Test : BaseCosmosDocument
        {

            [JsonProperty("ukprn")]
            public long Ukprn { get; set; }

            [JsonProperty("employerAccountId")]
            public long EmployerAccountId { get; set; }

        }


}
