using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Web.ServiceRegistrations;

public static class AddDatabaseRegistrationExtensions
{
    public static void AddDatabaseRegistration(this IServiceCollection services, IConfiguration config, string environmentName)
    {
        if (environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase) ||
            environmentName.Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
        {
            services.AddDbContext<ProviderRelationshipsDbContext>(
                options => options.UseSqlServer(config["ProviderRelationshipsWebConfiguration:DatabaseConnectionString"]),
                ServiceLifetime.Transient);
        }
        else
        {
            services.AddSingleton(new AzureServiceTokenProvider());
            services.AddDbContext<ProviderRelationshipsDbContext>(ServiceLifetime.Transient);
        }

        services.AddTransient(provider => provider.GetService<ProviderRelationshipsDbContext>());
        services.AddTransient(provider =>
            new Lazy<ProviderRelationshipsDbContext>(provider.GetService<ProviderRelationshipsDbContext>()));
    }
}