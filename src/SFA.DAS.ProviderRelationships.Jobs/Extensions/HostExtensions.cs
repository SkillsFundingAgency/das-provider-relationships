using NLog.Extensions.Logging;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Jobs.ServiceRegistrations;
using SFA.DAS.ProviderRelationships.ServiceRegistrations;
using SFA.DAS.UnitOfWork.DependencyResolution.Microsoft;

namespace SFA.DAS.ProviderRelationships.Jobs.Extensions;

public static class HostExtensions
{
    public static IHostBuilder ConfigureDasAppConfiguration(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureAppConfiguration((context, builder) =>
        {
            builder.AddAzureTableStorage(ConfigurationKeys.ProviderRelationships)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();
        });
    }

    public static IHostBuilder ConfigureDasLogging(this IHostBuilder builder)
    {
        
        builder.ConfigureLogging((context, loggingBuilder) =>
        {
            var appInsightsKey = context.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
            loggingBuilder.AddNLog(context.HostingEnvironment.IsDevelopment() ? "nlog.development.config" : "nlog.config");
            if (!string.IsNullOrEmpty(appInsightsKey))
            {
                loggingBuilder.AddApplicationInsightsWebJobs(o => o.InstrumentationKey = appInsightsKey);
            }
        });

        return builder;
    }


    public static IHostBuilder ConfigureDasWebJobs(this IHostBuilder builder)
    {
        builder.ConfigureWebJobs(config =>
        {
            config.AddTimers();
            config.AddAzureStorageCoreServices();
        });

        return builder;
    }

    public static IHostBuilder ConfigureDasServices(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddConfigurationSections(context.Configuration);
            services.AddNServiceBus();
            services.AddApplicationServices();
            services.AddDatabaseRegistration(context.Configuration[$"{ConfigurationKeys.ProviderRelationships}:DatabaseConnectionString"]);
            services.AddTransient<IRetryStrategy>(_ => new ExponentialBackoffRetryAttribute(5, "00:00:10", "00:00:20"));
            services.AddUnitOfWork();
#pragma warning disable 618
            services.AddSingleton<IWebHookProvider>(p => null);
#pragma warning restore 618
        });

        return hostBuilder;
    }
}