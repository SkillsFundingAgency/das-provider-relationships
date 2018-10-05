using SFA.DAS.ProviderRelationships.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.Data
{
    public interface IProviderRelationshipsDbContext
    {
        DbSet<Account> Accounts { get; }
        DbSet<AccountLegalEntity> AccountLegalEntities { get; }
        DbSet<Permission> Permissions { get; }
        DbSet<HealthCheck> HealthChecks { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();

        void Delete(object entity);
    }
}
