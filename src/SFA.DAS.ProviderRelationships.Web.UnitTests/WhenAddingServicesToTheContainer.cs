using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.ProviderRelationships.Application.Commands.RevokePermissions;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.ServiceRegistrations;
using SFA.DAS.ProviderRelationships.Web.AppStart;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.ServiceRegistrations;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests;

public class WhenAddingServicesToTheContainer
{
    [TestCase(typeof(AccountProviderLegalEntitiesController))]
    [TestCase(typeof(AccountProvidersController))]
    [TestCase(typeof(HealthCheckController))]
    [TestCase(typeof(HomeController))]
    [TestCase(typeof(ServiceController))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_Controllers(Type toResolve)
    {
        var serviceCollection = new ServiceCollection();
        SetupServiceCollection(serviceCollection);
        var provider = serviceCollection.BuildServiceProvider();

        var type = provider.GetService(toResolve);
        Assert.IsNotNull(type);
    }


    private static void SetupServiceCollection(IServiceCollection services)
    {
        var configuration = GenerateConfiguration();
        var relationshipsConfiguration = configuration
            .GetSection(ConfigurationKeys.ProviderRelationships)
            .Get<ProviderRelationshipsConfiguration>();

        services.AddSingleton(Mock.Of<IWebHostEnvironment>());
        services.AddMediatR(typeof(GetAccountProviderLegalEntityQuery));
        services.AddDatabaseRegistration(relationshipsConfiguration.DatabaseConnectionString);
        services.AddApplicationServices();
        services.AddConfigurationOptions(configuration);
        services.AddReadStoreServices();
        services.AddAutoMapper(typeof(Startup), typeof(AccountLegalEntityMappings));
        services.AddAutoConfiguration();

        services.AddTransient<AccountProviderLegalEntitiesController>();
        services.AddTransient<AccountProvidersController>();
        services.AddTransient<HealthCheckController>();
        services.AddTransient<HomeController>();
        services.AddTransient<ServiceController>();
    }

    private static IConfigurationRoot GenerateConfiguration()
    {
        var configSource = new MemoryConfigurationSource {
            InitialData = new List<KeyValuePair<string, string>>
            {
                new($"{ConfigurationKeys.ProviderRelationships}:CommitmentsApiV2ClientConfiguration:ApiBaseUrl", "https://test1.com/"),
                new($"{ConfigurationKeys.ProviderRelationships}:EmployerFinanceOuterApiConfiguration:BaseUrl", "https://test.com/"),
                new($"{ConfigurationKeys.ProviderRelationships}:EmployerFinanceOuterApiConfiguration:Key", "123edc"),
                new($"{ConfigurationKeys.ProviderRelationships}:DatabaseConnectionString", "Data Source=.;Initial Catalog=SFA.DAS.EmployerFinance;Integrated Security=True;Pooling=False;Connect Timeout=30"),
                new($"{ConfigurationKeys.ProviderRelationshipsReadStore}:Uri", "https://test.com"),
                new($"{ConfigurationKeys.ProviderRelationshipsReadStore}:AuthKey", "test"),
                new("SFA.DAS.Encoding", "{'Encodings':[{'EncodingType':'AccountId','Salt':'test','MinHashLength':6,'Alphabet':'46789BCDFGHJKLMNPRSTVWXY'}]}"),
                new("EnvironmentName", "test"),
            }
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}