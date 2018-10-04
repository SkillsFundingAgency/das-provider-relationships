using System.Data.Common;
using System.Data.Entity;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    [DbConfigurationType(typeof(SqlAzureDbConfiguration))]
    public class ProviderRelationshipsDbContext : DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountLegalEntity> AccountLegalEntities { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<HealthCheck> HealthChecks { get; set; }

        static ProviderRelationshipsDbContext()
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<ProviderRelationshipsDbContext>());
            Database.SetInitializer<ProviderRelationshipsDbContext>(null);
        }

        public ProviderRelationshipsDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public ProviderRelationshipsDbContext(DbConnection connection, DbTransaction transaction)
            : base(connection, false)
        {
            Database.UseTransaction(transaction);
        }

        protected ProviderRelationshipsDbContext()
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}