using CloneExtensions;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.Testing.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests
{
    public class TestProviderRelationshipsDbContext : IProviderRelationshipsDbContext
    {
        public TestProviderRelationshipsDbContext()
        {
            Accounts = new DbSetStub<Account>();
            AccountLegalEntities = new DbSetStub<AccountLegalEntity>();
            Permissions = new DbSetStub<Permission>();
            HealthChecks = new DbSetStub<HealthCheck>();
            Providers = new DbSetStub<Provider>();
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountLegalEntity> AccountLegalEntities { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<HealthCheck> HealthChecks { get; set; }
        public DbSet<Provider> Providers { get; set; }

        public int SaveChangesCount { get; private set; }

        public IEnumerable<Account> AccountsAtLastSaveChanges;
        public IEnumerable<AccountLegalEntity> AccountLegalEntitiesAtLastSaveChanges;
        public IEnumerable<Permission> PermissionsAtLastSaveChanges;
        public IEnumerable<HealthCheck> HealthChecksAtLastSaveChanges;
        public IEnumerable<Provider> ProvidersAtLastSaveChanges;

        public int SaveChanges()
        {
            AccountsAtLastSaveChanges = new List<Account>(Accounts).GetClone();
            AccountLegalEntitiesAtLastSaveChanges = new List<AccountLegalEntity>(AccountLegalEntities).GetClone();
            PermissionsAtLastSaveChanges = new List<Permission>(Permissions).GetClone();
            HealthChecksAtLastSaveChanges = new List<HealthCheck>(HealthChecks).GetClone();
            ProvidersAtLastSaveChanges = new List<Provider>(Providers).GetClone();

            ++SaveChangesCount;
            return 1;
        }

        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult(SaveChanges());
        }

        public void Delete(object entity)
        {
            //todo: could do this generically, if we duplicate how EF picks a key
            switch (entity)
            {
                case Account account:
                    Accounts = new DbSetStub<Account>(Accounts.Where(a => a.Id != account.Id));
                    break;
                case AccountLegalEntity accountLegalEntity:
                    AccountLegalEntities = new DbSetStub<AccountLegalEntity>(AccountLegalEntities.Where(ale => ale.Id != accountLegalEntity.Id));
                    break;
                case Permission permission:
                    Permissions = new DbSetStub<Permission>(Permissions.Where(p => p.PermissionId != permission.PermissionId));
                    break;
                case Provider provider:
                    Providers = new DbSetStub<Provider>(Providers.Where(p => p.UKPRN != provider.UKPRN));
                    break;
                case HealthCheck healthCheck:
                    HealthChecks = new DbSetStub<HealthCheck>(HealthChecks.Where(hc => hc.Id != healthCheck.Id));
                    break;
            }
        }
    }
}
