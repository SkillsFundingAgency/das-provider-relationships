using Microsoft.Azure.WebJobs.Logging.ApplicationInsights;
using SFA.DAS.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.ProviderRelationships.Application.Commands.AddAccountLegalEntity;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.MessageHandlers.ServiceRegistrations;
using SFA.DAS.ProviderRelationships.ServiceRegistrations;
using SFA.DAS.UnitOfWork.DependencyResolution.Microsoft;
using SFA.DAS.UnitOfWork.EntityFrameworkCore.DependencyResolution.Microsoft;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.Extensions;

public static class HostExtensions
{
    public static IHostBuilder UseDasEnvironment(this IHostBuilder hostBuilder)
    {
        var environmentName = Environment.GetEnvironmentVariable(EnvironmentVariableNames.EnvironmentName);
        var mappedEnvironmentName = DasEnvironmentName.Map[environmentName];

        return hostBuilder.UseEnvironment(mappedEnvironmentName);
    }

    public static IHostBuilder ConfigureDasLogging(this IHostBuilder builder)
    {
        builder.ConfigureLogging((context, loggingBuilder) =>
        {
            var connectionString = context.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
            if (!string.IsNullOrEmpty(connectionString))
            {
                loggingBuilder.AddNLog(context.HostingEnvironment.IsDevelopment()
                    ? "nlog.development.config"
                    : "nlog.config");
                loggingBuilder.AddApplicationInsightsWebJobs(o => o.ConnectionString = connectionString);
                loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Information);
                loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Information);
            }

            loggingBuilder.AddConsole();
        });

        return builder;
    }

    public static IHostBuilder ConfigureDasAppConfiguration(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureAppConfiguration((context, builder) =>
        {
            builder.AddAzureTableStorage(options =>
                    {
                        options.ConfigurationKeys = new[]
                            { ConfigurationKeys.ProviderRelationships, ConfigurationKeys.EncodingConfig };
                        options.PreFixConfigurationKeys = false;
                        options.ConfigurationKeysRawJsonResult = new[] { ConfigurationKeys.EncodingConfig };
                    }
                )
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();
        });
    }

    public static IHostBuilder ConfigureDasServices(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddConfigurationSections(context.Configuration);
            services.AddEntityFrameworkUnitOfWork<ProviderRelationshipsDbContext>();
            services.AddNServiceBus();
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(typeof(AddAccountLegalEntityCommand).Assembly,
                    typeof(Program).Assembly));
            services.AddApplicationServices();
            services.AddDatabaseRegistration(context.Configuration["DatabaseConnectionString"], ServiceLifetime.Scoped);
            services.AddUnitOfWork();
            services.AddTransient<IRetryStrategy>(_ => new ExponentialBackoffRetryAttribute(5, "00:00:10", "00:00:20"));
            services.BuildServiceProvider();
        });

        return hostBuilder;
    }
}