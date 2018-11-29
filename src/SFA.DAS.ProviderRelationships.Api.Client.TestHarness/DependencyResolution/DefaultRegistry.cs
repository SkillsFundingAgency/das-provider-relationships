using System.Collections.Specialized;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.ReadStore.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.TestHarness.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            //todo: does auto config handle everything now?
            
//            For<ProviderRelationshipsReadStoreConfiguration>().Use(() => new ProviderRelationshipsReadStoreConfiguration
//            {
//                Uri = "https://localhost:8081",
//                AuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
//            });

//            var appSettings = new NameValueCollection
//            {
//                {"ConfigurationStorageConnectionString", "UseDevelopmentStorage=true"},
//                {"EnvironmentName", "LOCAL"}
//            };
//            
//            For<IConfiguration>().Use<ProviderRelationships.Configuration.Configuration>().Ctor<NameValueCollection>().Is(appSettings);
//            For<ProviderRelationshipsApiClientConfiguration>().Use(c => c.GetInstance<IConfiguration>().Get<ProviderRelationshipsApiClientConfiguration>("SFA.DAS.ProviderRelationships.Api.Client")).Singleton();
        }
    }
}