using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.Http;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;
using SFA.DAS.ProviderRelationships.Api.Client.Http;
using SFA.DAS.ProviderRelationships.Api.Client.TestHarness.Scenarios;

namespace SFA.DAS.ProviderRelationships.Api.Client.TestHarness;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Provider Relationships Api Client Test Harness");

        var configuration = new ProviderRelationshipsApiConfiguration {
            // ApiBaseUrl = "https://das-at-prelapi-as.azurewebsites.net/",
            ApiBaseUrl = "https://localhost:44308/",
            IdentifierUri = "https://citizenazuresfabisgov.onmicrosoft.com/das-at-prelapi-as-ar"
        };

        using var host = BuildHost(args, configuration);

        try
        {
            await host.Services.GetService<PingScenario>().Run();
            await host.Services.GetService<GetAccountProviderLegalEntitiesWithPermissionScenario>().Run();
            await host.Services.GetService<HasPermissionScenario>().Run();
            await host.Services.GetService<HasRelationshipWithPermissionScenario>().Run();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        await host.RunAsync();
    }

    private static IHost BuildHost(string[] args, ProviderRelationshipsApiConfiguration configuration)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddSingleton<IProviderRelationshipsApiClient>(GetProviderRelationshipsApiClient(configuration));
                services.AddTransient<GetAccountProviderLegalEntitiesWithPermissionScenario>();
                services.AddTransient<HasPermissionScenario>();
                services.AddTransient<HasRelationshipWithPermissionScenario>();
                services.AddTransient<PingScenario>();

                services.AddSingleton(configuration);
            }).Build();
    }

    private static IProviderRelationshipsApiClient GetProviderRelationshipsApiClient(ProviderRelationshipsApiConfiguration configuration)
    {
        if (configuration.ApiBaseUrl.ToLower().Contains("localhost"))
        {
            var restHttpClient = new RestHttpClient(new HttpClient {
                BaseAddress = new Uri(configuration.ApiBaseUrl)
            });
            return new ProviderRelationshipsApiClient(restHttpClient);
        }

        var factory = new ProviderRelationshipsApiClientFactory(configuration);
        return factory.CreateApiClient();
    }
}