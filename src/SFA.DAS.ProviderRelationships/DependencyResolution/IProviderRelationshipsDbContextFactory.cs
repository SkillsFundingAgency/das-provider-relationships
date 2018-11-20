using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public interface IProviderRelationshipsDbContextFactory
    {
        ProviderRelationshipsDbContext CreateDbContext();
    }
}