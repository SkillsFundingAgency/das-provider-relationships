using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data
{
    public class ProviderRelationshipsDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountLegalEntity> AccountLegalEntities { get; set; }
        public DbSet<HealthCheck> HealthChecks { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<User> Users { get; set; }
        
        public ProviderRelationshipsDbContext(DbContextOptions<ProviderRelationshipsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().Property(a => a.Id).ValueGeneratedNever();
            modelBuilder.Entity<AccountLegalEntity>().Property(a => a.Id).ValueGeneratedNever();
            modelBuilder.Entity<AccountLegalEntityProvider>().HasKey(ap => new { ap.AccountLegalEntityId, ap.Ukprn });
            
            modelBuilder.Entity<AccountLegalEntityProvider>()
                .HasOne(ap => ap.AccountLegalEntity)
                .WithMany(ap => ap.AccountLegalEntityProviders)
                .HasForeignKey(ap => ap.AccountLegalEntityId);
            
            modelBuilder.Entity<AccountLegalEntityProvider>()
                .HasOne(ap => ap.Provider)
                .WithMany(ap => ap.AccountLegalEntityProviders)
                .HasForeignKey(ap => ap.Ukprn);
            
            modelBuilder.Entity<Provider>().HasKey(p => p.Ukprn);
            modelBuilder.Entity<Provider>().Property(a => a.Ukprn).ValueGeneratedNever();
            modelBuilder.Entity<User>().HasKey(u => u.Ref);
            modelBuilder.Entity<User>().Property(u => u.Ref).ValueGeneratedNever();
        }
    }
}