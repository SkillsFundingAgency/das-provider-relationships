using System.Data.Common;
using System.Data.Entity;

namespace SFA.DAS.ProviderRelationships.Data
{
    [DbConfigurationType(typeof(SqlAzureDbConfiguration))]
    public class ProviderRelationshipsDbContext : DbContext
    {
        static ProviderRelationshipsDbContext()
        {
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