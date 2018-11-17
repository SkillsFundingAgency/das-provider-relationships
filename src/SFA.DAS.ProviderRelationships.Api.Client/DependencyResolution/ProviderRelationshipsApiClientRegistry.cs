using SFA.DAS.ProviderRelationships.ReadStore.DependencyResolution;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution
{
    public class ProviderRelationshipsApiClientRegistry : Registry
    {
        public ProviderRelationshipsApiClientRegistry()
        {
            IncludeRegistry<ReadStoreDataRegistry>();
            IncludeRegistry<ReadStoreMediatorRegistry>();
            For<IProviderRelationshipsApiClient>().Use<ProviderRelationshipsApiClient>();
        }
    }
}