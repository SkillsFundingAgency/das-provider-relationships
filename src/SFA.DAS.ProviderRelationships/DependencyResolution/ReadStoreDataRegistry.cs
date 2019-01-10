using Microsoft.Azure.Documents;
using StructureMap;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using DocumentClientFactory = SFA.DAS.ProviderRelationships.ReadStore.Data.DocumentClientFactory;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class ReadStoreDataRegistry : Registry
    {
        public ReadStoreDataRegistry()
        {
            For<IDocumentClient>().Add(c => c.GetInstance<IDocumentClientFactory>().CreateDocumentClient()).Named(GetType().FullName).Singleton();
            For<IDocumentClientFactory>().Use<DocumentClientFactory>();
            For<IAccountProviderLegalEntitiesRepository>().Use<AccountProviderLegalEntitiesRepository>().Ctor<IDocumentClient>().IsNamedInstance(GetType().FullName);
        }
    }
}