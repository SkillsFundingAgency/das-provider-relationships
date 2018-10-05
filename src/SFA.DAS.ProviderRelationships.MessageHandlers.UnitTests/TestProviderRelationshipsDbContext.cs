using CloneExtensions;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests
{
    public class TestProviderRelationshipsDbContext : IProviderRelationshipsDbContext
    {
        public TestProviderRelationshipsDbContext()
        {
            Accounts = new DbSetStubX<Account>();
            AccountLegalEntities = new DbSetStubX<AccountLegalEntity>();
            Permissions = new DbSetStubX<Permission>();
            HealthChecks = new DbSetStubX<HealthCheck>();
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountLegalEntity> AccountLegalEntities { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<HealthCheck> HealthChecks { get; set; }

        public int SaveChangesCount { get; private set; }

        public IEnumerable<Account> AccountsAtLastSaveChanges;
        public IEnumerable<AccountLegalEntity> AccountLegalEntitiesAtLastSaveChanges;
        public IEnumerable<Permission> PermissionsAtLastSaveChanges;
        public IEnumerable<HealthCheck> HealthChecksAtLastSaveChanges;

        public int SaveChanges()
        {
            AccountsAtLastSaveChanges = new List<Account>(Accounts).GetClone();
            AccountLegalEntitiesAtLastSaveChanges = new List<AccountLegalEntity>(AccountLegalEntities).GetClone();
            PermissionsAtLastSaveChanges = new List<Permission>(Permissions).GetClone();
            HealthChecksAtLastSaveChanges = new List<HealthCheck>(HealthChecks).GetClone();

            ++SaveChangesCount;
            return 1;
        }

        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult(SaveChanges());
        }

        public void Delete(object entity)
        {
            switch (entity)
            {
                case Account account:
                    Accounts.Remove(account);
                    break;
                case AccountLegalEntity accountLegalEntity:
                    AccountLegalEntities.Remove(accountLegalEntity);
                    break;
                case Permission permission:
                    Permissions.Remove(permission);
                    break;
                case HealthCheck healthCheck:
                    HealthChecks.Remove(healthCheck);
                    break;
            }
        }
    }
}
