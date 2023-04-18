using System.IO;
using Microsoft.Extensions.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.ProviderRelationships.Configuration;

namespace SFA.DAS.ProviderRelationships.Web.Extensions;

public static class ConfigurationExtensions
{
    public static bool UseGovUkSignIn(this IConfiguration configuration)
    {
        return configuration["UseGovUkSignIn"] != null &&
               configuration["UseGovUkSignIn"].Equals("true", StringComparison.CurrentCultureIgnoreCase);
    }

    public static bool UseStubAuth(this IConfiguration configuration)
    {
        return configuration["StubAuth"] != null && configuration["StubAuth"]
            .Equals("true", StringComparison.CurrentCultureIgnoreCase);
    }

    public static IConfigurationRoot BuildDasConfiguration(this IConfiguration configuration)
    {
        var configurationBuilder = new ConfigurationBuilder()
            .AddConfiguration(configuration)
            .SetBasePath(Directory.GetCurrentDirectory());
#if DEBUG
        if (!configuration.IsDev())
        {
            configurationBuilder.AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.Development.json", true);
        }
#endif

        configurationBuilder.AddEnvironmentVariables();
        if (!configuration.IsDev())
        {
            configurationBuilder.AddAzureTableStorage(options =>
                {
                    options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                    options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                    options.EnvironmentName = configuration["EnvironmentName"];
                    options.PreFixConfigurationKeys = false;
                    options.ConfigurationKeysRawJsonResult = new[] { "SFA.DAS.Encoding" };
                }
            );
        }
        return configurationBuilder.Build();
    }
}