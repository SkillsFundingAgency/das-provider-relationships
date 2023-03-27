using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data.Configuration;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Data;

public class ProviderRelationshipsDbContext : DbContext
{
    private readonly ProviderRelationshipsConfiguration _configuration;
    private readonly AzureServiceTokenProvider _azureServiceTokenProvider;
    private readonly IDbConnection _connection;

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

    public ProviderRelationshipsDbContext(DbContextOptions<ProviderRelationshipsDbContext> options) : base(options)
    {
    }

    public ProviderRelationshipsDbContext(IDbConnection connection, ProviderRelationshipsConfiguration configuration, DbContextOptions options, AzureServiceTokenProvider azureServiceTokenProvider) : base(options)
    {
        _configuration = configuration;
        _azureServiceTokenProvider = azureServiceTokenProvider;
        _connection = connection;
    }

    protected ProviderRelationshipsDbContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // To allow use of InMemoryProvider in unit tests
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        if (_configuration == null || _azureServiceTokenProvider == null)
        {
            optionsBuilder.UseSqlServer().UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            return;
        }

        optionsBuilder.UseSqlServer(_connection as DbConnection);
    }

    public virtual Task ExecuteSqlCommandAsync(string sql, params object[] parameters)
    {
        return Database.ExecuteSqlRawAsync(sql, parameters);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new AccountLegalEntityConfiguration());
        modelBuilder.ApplyConfiguration(new AccountProviderConfiguration());
        modelBuilder.ApplyConfiguration(new AccountProviderLegalEntityConfiguration());
        modelBuilder.ApplyConfiguration(new HealthCheckConfiguration());
        modelBuilder.ApplyConfiguration(new ProviderConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UpdatedPermissionsEventAuditConfiguration());
    }
}