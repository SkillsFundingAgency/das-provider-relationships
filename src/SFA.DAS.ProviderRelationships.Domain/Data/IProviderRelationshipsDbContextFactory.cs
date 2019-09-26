namespace SFA.DAS.ProviderRelationships.Domain.Data
{
    public interface IProviderRelationshipsDbContextFactory
    {
        ProviderRelationshipsDbContext CreateDbContext();
    }
}