using Microsoft.Azure.Documents;
using StructureMap;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution
{
    internal class ReadStoreDataRegistry : Registry
    {
        public ReadStoreDataRegistry()
        {
            For<IDocumentClient>().Add(c => c.GetInstance<IDocumentClientFactory>().CreateDocumentClient()).Named(GetType().FullName).Singleton();
            For<IDocumentClientFactory>().Use<DocumentClientFactory>();
            For<IAccountProviderLegalEntitiesReadOnlyRepository>().Use<AccountProviderLegalEntitiesReadOnlyRepository>().Ctor<IDocumentClient>().IsNamedInstance(GetType().FullName);
        }
    }
}