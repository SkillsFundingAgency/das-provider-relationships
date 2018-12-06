using SFA.DAS.ProviderRelationships.Data;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<IProviderRelationshipsDbContextFactory>().Use<DefaultTransactionDbContextFactory>();
        }
    }
}