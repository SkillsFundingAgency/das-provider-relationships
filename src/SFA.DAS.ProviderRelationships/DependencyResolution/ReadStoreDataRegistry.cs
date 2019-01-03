using Microsoft.Azure.Documents;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using StructureMap;

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