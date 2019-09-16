using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution.StructureMap
{
    internal class ReadStoreDataRegistry : Registry
    {
        public ReadStoreDataRegistry()
        {
            For<IDocumentClientFactory>().Use<DocumentClientFactory>().Singleton();
            For<IAccountProviderLegalEntitiesReadOnlyRepository>().Use<AccountProviderLegalEntitiesReadOnlyRepository>().Transient();
        }
    }
}