using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Domain.Models;

namespace SFA.DAS.ProviderRelationships.Domain.Data
{
    public class ProviderRelationshipsDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountLegalEntity> AccountLegalEntities { get; set; }
        public DbSet<AccountProvider> AccountProviders { get; set; }
        public DbSet<AccountProviderLegalEntity> AccountProviderLegalEntities { get; set; }
        public DbSet<HealthCheck> HealthChecks { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UpdatedPermissionsEventAudit> UpdatedPermissionsEventAudits { get; set; }
        public DbSet<DeletedPermissionsEventAudit> DeletedPermissionsEventAudits { get; set; }
        public DbSet<AddedAccountProviderEventAudit> AddedAccountProviderEventAudits { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        public ProviderRelationshipsDbContext(DbContextOptions<ProviderRelationshipsDbContext> options) : base(options)
        {
        }

        protected ProviderRelationshipsDbContext()
        {
        }

        public virtual Task ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return Database.ExecuteSqlCommandAsync(sql, parameters);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new AccountLegalEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AccountProviderConfiguration());
            modelBuilder.ApplyConfiguration(new AccountProviderLegalEntityConfiguration());
            modelBuilder.ApplyConfiguration(new HealthCheckConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new ProviderConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UpdatedPermissionsEventAuditConfiguration());
            modelBuilder.ApplyConfiguration(new DeletedPermissionsEventAuditConfiguration());
            modelBuilder.ApplyConfiguration(new AddedAccountProviderEventAuditConfiguration());
            modelBuilder.ApplyConfiguration(new InvitationConfiguration());
        }
    }
}