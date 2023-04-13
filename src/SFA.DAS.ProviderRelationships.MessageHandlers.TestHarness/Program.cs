using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.Extensions;
using SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness.Scenarios;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.TestHarness;

public static class Program
{
    public static async Task Main()
    {
        var provider = RegisterServices();

        await provider.GetService<PublishEmployerAccountsEventsScenario>().Run();
        await provider.GetService<PublishProviderRelationshipsEventsScenario>().Run();
    }

    private static IServiceProvider RegisterServices()
    {
        var configuration = GenerateConfiguration();
        var financeConfiguration = configuration
            .GetSection(ConfigurationKeys.ProviderRelationships)
            .Get<ProviderRelationshipsConfiguration>();

        return new ServiceCollection()
            .AddNServiceBus()
            .AddSingleton<PublishEmployerAccountsEventsScenario>()
            .AddSingleton<PublishProviderRelationshipsEventsScenario>()
            .AddSingleton<IConfiguration>(configuration)
            .AddSingleton(financeConfiguration)
            .BuildServiceProvider();
    }

    private static IConfigurationRoot GenerateConfiguration()
    {
        var configSource = new MemoryConfigurationSource {
            InitialData = new List<KeyValuePair<string, string>>
            {
                new("SFA.DAS.ProviderRelationshipsV2:DatabaseConnectionString", "Data Source=.;Initial Catalog=Testing;Integrated Security=True;Pooling=False;Connect Timeout=30"),
                new("SFA.DAS.ProviderRelationshipsV2:ServiceBusConnectionString", "test"),
                new("SFA.DAS.ProviderRelationshipsV2:NServiceBusLicense", "test"),
                new("SFA.DAS.ProviderRelationshipsV2:RoatpApiClientSettings:ApiBaseUrl", "https://test/"),
                new("SFA.DAS.ProviderRelationshipsV2:RoatpApiClientSettings:IdentifierUri", "https://test/"),
                new("SFA.DAS.ProviderRelationshipsV2:ReadStore:AuthKey", "test"),
                new("SFA.DAS.ProviderRelationshipsV2:ReadStore:Uri", "https://test/"),
                new("EnvironmentName", "LOCAL"),
            }
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}