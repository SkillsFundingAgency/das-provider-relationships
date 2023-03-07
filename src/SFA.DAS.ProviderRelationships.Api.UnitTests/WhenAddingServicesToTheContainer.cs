using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Api.Controllers;
using SFA.DAS.ProviderRelationships.Api.ServiceRegistrations;
using SFA.DAS.ProviderRelationships.Application.Commands.RevokePermissions;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntitiesWithPermission;
using SFA.DAS.ProviderRelationships.Application.Queries.HasPermission;
using SFA.DAS.ProviderRelationships.Application.Queries.HasRelationshipWithPermission;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.ServiceRegistrations;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests;

public class WhenAddingServicesToTheContainer
{
    [TestCase(typeof(AccountProviderLegalEntitiesController))]
    [TestCase(typeof(PermissionsController))]
    [TestCase(typeof(PingController))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_Controllers(Type toResolve)
    {
        RunTestForType(toResolve);
    }

    [TestCase(typeof(IRequestHandler<GetAccountProviderLegalEntitiesWithPermissionQuery, GetAccountProviderLegalEntitiesWithPermissionQueryResult>))]
    [TestCase(typeof(IRequestHandler<HasPermissionQuery, bool>))]
    [TestCase(typeof(IRequestHandler<HasRelationshipWithPermissionQuery, bool>))]
    [TestCase(typeof(IRequestHandler<RevokePermissionsCommand, Unit>))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_Mediator_Handlers(Type toResolve)
    {
       RunTestForType(toResolve);
    }

    private static void RunTestForType(Type toResolve)
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
        services.AddMediatR(typeof(RevokePermissionsCommand));
        services.AddDatabaseRegistration(relationshipsConfiguration.DatabaseConnectionString);
        services.AddApplicationServices();
        services.AddConfigurationSections(configuration);
        services.AddReadStoreServices();
        services.AddAutoMapper(typeof(Startup), typeof(AccountLegalEntityMappings));

        services.AddTransient<AccountProviderLegalEntitiesController>();
        services.AddTransient<PermissionsController>();
        services.AddTransient<PingController>();
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
                new("ReadStore:Uri", "https://test.com"),
                new("ReadStore:AuthKey", "test"),
                new("SFA.DAS.Encoding", "{'Encodings':[{'EncodingType':'AccountId','Salt':'test','MinHashLength':6,'Alphabet':'46789BCDFGHJKLMNPRSTVWXY'}]}"),
                new("EnvironmentName", "test"),
            }
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}