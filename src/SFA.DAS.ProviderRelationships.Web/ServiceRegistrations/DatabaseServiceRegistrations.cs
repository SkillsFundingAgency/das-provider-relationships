using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.Persistence;
using SFA.DAS.NServiceBus.SqlServer.Data;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.UnitOfWork.Context;

namespace SFA.DAS.ProviderRelationships.Web.ServiceRegistrations;

public static class AddDatabaseRegistrationExtensions
{
    public static IServiceCollection AddEntityFramework(this IServiceCollection services, ProviderRelationshipsConfiguration config)
    {
        return services.AddScoped(p =>
        {
            var unitOfWorkContext = p.GetService<IUnitOfWorkContext>();
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            ProviderRelationshipsDbContext dbContext;
            try
            {
                var synchronizedStorageSession = unitOfWorkContext.Get<SynchronizedStorageSession>();
                var sqlStorageSession = synchronizedStorageSession.GetSqlStorageSession();
                var optionsBuilder = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseSqlServer(sqlStorageSession.Connection);
                dbContext = new ProviderRelationshipsDbContext(sqlStorageSession.Connection, config, optionsBuilder.Options, azureServiceTokenProvider);
                dbContext.Database.UseTransaction(sqlStorageSession.Transaction);
            }
            catch (KeyNotFoundException)
            {
                var connection = DatabaseExtensions.GetSqlConnection(config.DatabaseConnectionString);
                var optionsBuilder = new DbContextOptionsBuilder<ProviderRelationshipsDbContext>().UseSqlServer(connection);
                dbContext = new ProviderRelationshipsDbContext(optionsBuilder.Options);
            }

            return dbContext;
        });
    }
}