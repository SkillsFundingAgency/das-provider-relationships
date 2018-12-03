namespace SFA.DAS.ProviderRelationships.Data
{
    public interface IProviderRelationshipsDbContextFactory
    {
        ProviderRelationshipsDbContext CreateDbContext();
    }
}