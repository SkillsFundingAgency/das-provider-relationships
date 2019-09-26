using SFA.DAS.ProviderRelationships.Domain.Data;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<IProviderRelationshipsDbContextFactory>().Use<DbContextWithNewTransactionFactory>();
        }
    }
}