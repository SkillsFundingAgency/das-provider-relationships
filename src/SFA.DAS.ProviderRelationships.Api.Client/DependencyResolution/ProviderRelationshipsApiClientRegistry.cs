using StructureMap;
using SFA.DAS.AutoConfiguration.DependencyResolution;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution
{
    public class ProviderRelationshipsApiClientRegistry : Registry
    {
        public ProviderRelationshipsApiClientRegistry()
        {
            IncludeRegistry<AutoConfigurationRegistry>();
            IncludeRegistry<ReadStoreDataRegistry>();
            IncludeRegistry<MediatorRegistry>();
            IncludeRegistry<HttpRegistry>();
            IncludeRegistry<ConfigurationRegistry>();
        }
    }
}