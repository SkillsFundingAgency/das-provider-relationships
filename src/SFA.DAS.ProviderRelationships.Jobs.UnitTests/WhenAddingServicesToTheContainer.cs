using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Jobs.Extensions;
using SFA.DAS.ProviderRelationships.Jobs.ScheduledJobs;
using SFA.DAS.ProviderRelationships.Jobs.ServiceRegistrations;
using SFA.DAS.ProviderRelationships.Jobs.StartupJobs;
using SFA.DAS.ProviderRelationships.ServiceRegistrations;
using SFA.DAS.UnitOfWork.DependencyResolution.Microsoft;

namespace SFA.DAS.ProviderRelationships.Jobs.UnitTests;

public class WhenAddingServicesToTheContainer
{
    [TestCase(typeof(ImportProvidersJob))]
    [TestCase(typeof(CreateReadStoreDatabaseJob))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_Jobs(Type toResolve)
    {
        var services = new ServiceCollection();
        SetupServiceCollection(services);
        var provider = services.BuildServiceProvider();

        var type = provider.GetService(toResolve);
        Assert.IsNotNull(type);
    }

    private static void SetupServiceCollection(IServiceCollection services)
    {
        var configuration = GenerateConfiguration();
        var relationshipsConfiguration = configuration
            .GetSection("SFA.DAS.ProviderRelationshipsV2")
            .Get<ProviderRelationshipsConfiguration>();

        services.AddSingleton<IConfiguration>(configuration);
        services.AddConfigurationSections(configuration);
        services.AddNServiceBus();
        services.AddDatabaseRegistration(relationshipsConfiguration.DatabaseConnectionString);
        services.AddUnitOfWork();
        services.AddMediatR(typeof(Program));
        services.AddLogging(_ => { });
        services.AddApplicationServices();
        services.AddTransient<ImportProvidersJob>();
        services.AddTransient<CreateReadStoreDatabaseJob>();
    }


    private static IConfigurationRoot GenerateConfiguration()
    {
        var configSource = new MemoryConfigurationSource
        {
            InitialData = new List<KeyValuePair<string, string>>
            {
                new($"{ConfigurationKeys.ProviderRelationships}:DatabaseConnectionString", "Data Source=.;Initial Catalog=Testing;Integrated Security=True;Pooling=False;Connect Timeout=30"),
                new($"{ConfigurationKeys.ProviderRelationships}:ServiceBusConnectionString", "test"),
                new($"{ConfigurationKeys.ProviderRelationships}:NServiceBusLicense", "test"),
                new($"{ConfigurationKeys.ProviderRelationships}:RoatpApiClientSettings:ApiBaseUrl", "http://test/"),
                new($"{ConfigurationKeys.ProviderRelationships}:RoatpApiClientSettings:IdentifierUri", "http://test/"),
                new($"{ConfigurationKeys.ProviderRelationshipsReadStore}:AuthKey", "test"),
                new($"{ConfigurationKeys.ProviderRelationshipsReadStore}:Uri", "http://test/"),
                new("EnvironmentName", "LOCAL"),
            }
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}