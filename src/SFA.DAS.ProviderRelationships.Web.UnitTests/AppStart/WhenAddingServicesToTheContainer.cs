using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.ProviderRelationships.Application.Commands.AddAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Commands.RunHealthCheck;
using SFA.DAS.ProviderRelationships.Application.Commands.UpdatePermissions;
using SFA.DAS.ProviderRelationships.Application.Queries.FindProviderToAdd;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAddedAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetHealthCheck;
using SFA.DAS.ProviderRelationships.Application.Queries.GetInvitationByIdQuery;
using SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Mappings;
using SFA.DAS.ProviderRelationships.ServiceRegistrations;
using SFA.DAS.ProviderRelationships.Web.Authorisation.Handlers;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.ProviderRelationships.Web.ServiceRegistrations;

namespace SFA.DAS.ProviderRelationships.Web.UnitTests.AppStart;

public class WhenAddingServicesToTheContainer
{
    [TestCase(typeof(AccountProviderLegalEntitiesController))]
    [TestCase(typeof(AccountProvidersController))]
    [TestCase(typeof(HealthCheckController))]
    [TestCase(typeof(HomeController))]
    [TestCase(typeof(ServiceController))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_Controllers(Type toResolve)
    {
        RunTestForType(toResolve);
    }

    [TestCase(typeof(IRequestHandler<GetAccountProviderLegalEntityQuery, GetAccountProviderLegalEntityQueryResult>))]
    [TestCase(typeof(IRequestHandler<GetAccountProviderQuery, GetAccountProviderQueryResult>))]
    [TestCase(typeof(IRequestHandler<GetAccountProviderQuery, GetAccountProviderQueryResult>))]
    [TestCase(typeof(IRequestHandler<FindProviderToAddQuery, FindProviderToAddQueryResult>))]
    [TestCase(typeof(IRequestHandler<GetProviderToAddQuery, GetProviderToAddQueryResult>))]
    [TestCase(typeof(IRequestHandler<GetAddedAccountProviderQuery, GetAddedAccountProviderQueryResult>))]
    [TestCase(typeof(IRequestHandler<GetInvitationByIdQuery, GetInvitationByIdQueryResult>))]
    [TestCase(typeof(IRequestHandler<FindProviderToAddQuery, FindProviderToAddQueryResult>))]
    [TestCase(typeof(IRequestHandler<GetHealthCheckQuery, GetHealthCheckQueryResult>))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_MediatorQueryHandlers(Type toResolve)
    {
        RunTestForType(toResolve);
    }

    [TestCase(typeof(IRequestHandler<UpdatePermissionsCommand, Unit>))]
    [TestCase(typeof(IRequestHandler<AddAccountProviderCommand, long>))]
    [TestCase(typeof(IRequestHandler<AddAccountProviderCommand, long>))]
    [TestCase(typeof(IRequestHandler<RunHealthCheckCommand, Unit>))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_MediatorCommandHandlers(Type toResolve)
    {
        RunTestForType(toResolve);
    }

    [TestCase(typeof(IEmployerAccountAuthorisationHandler))]
    [TestCase(typeof(ICustomClaims))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_AuthServices(Type toResolve)
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

    [Test]
    public void Then_Resolves_Authorization_Handlers()
    {
        var serviceCollection = new ServiceCollection();
        SetupServiceCollection(serviceCollection);
        var provider = serviceCollection.BuildServiceProvider();

        var type = provider.GetServices(typeof(IAuthorizationHandler)).ToList();

        type.Should().NotBeNull();
        type.Should().ContainSingle(c => c.GetType() == typeof(EmployerAllRolesAuthorizationHandler));
        type.Should().ContainSingle(c => c.GetType() == typeof(EmployerOwnerAuthorizationHandler));
        type.Should().ContainSingle(c => c.GetType() == typeof(EmployerViewerAuthorizationHandler));
    }

    private static void SetupServiceCollection(IServiceCollection services)
    {
        var configuration = GenerateConfiguration();
        var relationshipsConfiguration = configuration
            .GetSection(ConfigurationKeys.ProviderRelationships)
            .Get<ProviderRelationshipsConfiguration>();

        services.AddSingleton<IConfiguration>(configuration);
        services.AddHttpContextAccessor();

        services.AddLogging();
        services.AddSingleton(Mock.Of<IWebHostEnvironment>());
        services.AddMediatR(typeof(GetAccountProviderLegalEntityQuery));
        services.AddDatabaseRegistration(relationshipsConfiguration.DatabaseConnectionString);
        services.AddApplicationServices();
        services.AddApiClients();
        services.AddAuthenticationServices();
        services.AddConfigurationOptions(configuration);
        services.AddReadStoreServices();
        services.AddAutoMapper(typeof(Startup), typeof(AccountLegalEntityMappings));
        services.AddAutoConfiguration();

        AddControllers(services);

        services.AddTransient(_ => CreateAuthorizationContext());
    }

    private static void AddControllers(IServiceCollection services)
    {
        services.AddTransient<AccountProviderLegalEntitiesController>();
        services.AddTransient<AccountProvidersController>();
        services.AddTransient<HealthCheckController>();
        services.AddTransient<HomeController>();
        services.AddTransient<ServiceController>();
    }

    private static AuthorizationHandlerContext CreateAuthorizationContext()
    {
        var resource = new { Name = "test" };
        var user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new(ClaimTypes.Name, "test.object") }));
        var requirement = new OperationAuthorizationRequirement { Name = "Read" };

        return new AuthorizationHandlerContext(new List<IAuthorizationRequirement> { requirement }, user, resource);
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
                new("RoatpApiClientSettings:ApiBaseUrl", "https://test.com"),
                new("RoatpApiClientSettings:IdentifierUri", "https://test.com"),
                new("ProviderRelationshipsApi:ApiBaseUrl", "https://test.com"),
                new("ProviderRelationshipsApi:IdentifierUri", "https://test.com"),
                new("RecruitApiClientConfiguration:ApiBaseUrl", "https://test.com"),
                new("RecruitApiClientConfiguration:IdentifierUri", "https://test.com"),
                new("RegistrationApiClientConfiguration:ApiBaseUrl", "https://test.com"),
                new("RegistrationApiClientConfiguration:IdentifierUri", "https://test.com"),
                new($"{nameof(OuterApiConfiguration)}:BaseUrl", "https://test.com"),
                new($"{nameof(OuterApiConfiguration)}:Key", "test"),
                new("SFA.DAS.Encoding", "{'Encodings':[{'EncodingType':'AccountId','Salt':'test','MinHashLength':6,'Alphabet':'46789BCDFGHJKLMNPRSTVWXY'}]}"),
                new("EnvironmentName", "test"),
            }
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}