using SFA.DAS.AutoConfiguration.DependencyResolution;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution.StructureMap
{
    public class ProviderRelationshipsApiClientRegistry : Registry
    {
        public ProviderRelationshipsApiClientRegistry()
        {
            IncludeRegistry<AutoConfigurationRegistry>();
            IncludeRegistry<ConfigurationRegistry>();
            IncludeRegistry<HttpRegistry>();
            IncludeRegistry<MediatorRegistry>();
            IncludeRegistry<ReadStoreDataRegistry>();
        }
    }
}