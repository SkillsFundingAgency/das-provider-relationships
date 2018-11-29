using SFA.DAS.ProviderRelationships.Api.Data;
using SFA.DAS.ProviderRelationships.Data;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.DependencyResolution
{
    public class ApiDataRegistry : Registry
    {
        public ApiDataRegistry()
        {
            For<IProviderRelationshipsDbContextFactory>().Use<ProviderRelationshipsApiDbContextFactory>();
            For<ProviderRelationshipsDbContext>().Use(c => c.GetInstance<IProviderRelationshipsDbContextFactory>().CreateDbContext());
        }
    }
}