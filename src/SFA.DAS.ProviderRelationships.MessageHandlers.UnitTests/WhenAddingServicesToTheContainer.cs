using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Application.Commands.AddAccountLegalEntity;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateAccount;
using SFA.DAS.ProviderRelationships.Application.Commands.ReceiveProviderRelationshipsHealthCheckEvent;
using SFA.DAS.ProviderRelationships.Application.Commands.RemoveAccountLegalEntity;
using SFA.DAS.ProviderRelationships.Application.Commands.SendDeletedPermissionsNotification;
using SFA.DAS.ProviderRelationships.Application.Commands.UpdateAccountLegalEntityName;
using SFA.DAS.ProviderRelationships.Application.Commands.UpdateAccountName;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;
using SFA.DAS.ProviderRelationships.MessageHandlers.Extensions;
using SFA.DAS.ProviderRelationships.MessageHandlers.ServiceRegistrations;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.DeletePermissions;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.UpdatePermissions;
using SFA.DAS.ProviderRelationships.ServiceRegistrations;
using SFA.DAS.UnitOfWork.DependencyResolution.Microsoft;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.UnitTests;

public class WhenAddingServicesToTheContainer
{
    [TestCase(typeof(AddedLegalEntityEventHandler))]
    [TestCase(typeof(ChangedAccountNameEventHandler))]
    [TestCase(typeof(CreatedAccountEventHandler))]
    [TestCase(typeof(RemovedLegalEntityEventHandler))]
    [TestCase(typeof(UpdatedLegalEntityEventHandler))]
    [TestCase(typeof(AddedAccountProviderEventHandler))]
    [TestCase(typeof(DeletedPermissionsEventHandler))]
    [TestCase(typeof(DeletedPermissionsEventV2Handler))]
    [TestCase(typeof(HealthCheckEventHandler))]
    [TestCase(typeof(UpdatedPermissionsEventHandler))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_EventHandlers(Type toResolve)
    {
        var services = new ServiceCollection();
        SetupServiceCollection(services);
        var provider = services.BuildServiceProvider();

        var type = provider.GetService(toResolve);
        Assert.IsNotNull(type);
    }

    [TestCase(typeof(IRequestHandler<AddAccountLegalEntityCommand, Unit>))]
    [TestCase(typeof(IRequestHandler<UpdateAccountNameCommand, Unit>))]
    [TestCase(typeof(IRequestHandler<CreateAccountCommand, Unit>))]
    [TestCase(typeof(IRequestHandler<RemoveAccountLegalEntityCommand, Unit>))]
    [TestCase(typeof(IRequestHandler<UpdateAccountLegalEntityNameCommand, Unit>))]
    [TestCase(typeof(IRequestHandler<AddedAccountProviderEventAuditCommand, Unit>))]
    [TestCase(typeof(IRequestHandler<DeletedPermissionsEventAuditCommand, Unit>))]
    [TestCase(typeof(IRequestHandler<DeletePermissionsCommand, Unit>))]
    [TestCase(typeof(IRequestHandler<SendDeletedPermissionsNotificationCommand, Unit>))]
    [TestCase(typeof(IRequestHandler<ReceiveProviderRelationshipsHealthCheckEventCommand, Unit>))]
    [TestCase(typeof(IRequestHandler<UpdatedPermissionsEventAuditCommand, Unit>))]
    [TestCase(typeof(IRequestHandler<UpdatePermissionsCommand, Unit>))]
    public void Then_The_Dependencies_Are_Correctly_Resolved_For_CommandHandlers(Type toResolve)
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
        services.AddMediatR(typeof(UpdatePermissionsCommand));
        services.AddLogging(_ => { });
        services.AddApplicationServices();

        RegisterEventHandlers(services);
    }

    private static void RegisterEventHandlers(IServiceCollection services)
    {
        var handlersAssembly = typeof(AddedLegalEntityEventHandler).Assembly;
        var handlerTypes = handlersAssembly
            .GetTypes()
            .Where(x => x.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandleMessages<>)));

        foreach (var handlerType in handlerTypes)
        {
            services.AddTransient(handlerType);
        }
    }


    private static IConfigurationRoot GenerateConfiguration()
    {
        var configSource = new MemoryConfigurationSource {
            InitialData = new List<KeyValuePair<string, string>>
            {
                new("SFA.DAS.ProviderRelationshipsV2:DatabaseConnectionString", "Data Source=.;Initial Catalog=Testing;Integrated Security=True;Pooling=False;Connect Timeout=30"),
                new("SFA.DAS.ProviderRelationshipsV2:ServiceBusConnectionString", "test"),
                new("SFA.DAS.ProviderRelationshipsV2:NServiceBusLicense", "test"),
                new("SFA.DAS.ProviderRelationshipsV2:RoatpApiClientSettings:ApiBaseUrl", "http://test/"),
                new("SFA.DAS.ProviderRelationshipsV2:RoatpApiClientSettings:IdentifierUri", "http://test/"),
                new("SFA.DAS.ProviderRelationshipsV2:ReadStore:AuthKey", "test"),
                new("SFA.DAS.ProviderRelationshipsV2:ReadStore:Uri", "http://test/"),
                new("SFA.DAS.ProviderRelationshipsV2:PasAccountApi:ApiBaseUrl", "http://test/"),
                new("SFA.DAS.ProviderRelationshipsV2:PasAccountApi:IdentifierUri", "http://test/"),
                new("EnvironmentName", "LOCAL"),
            }
        };

        var provider = new MemoryConfigurationProvider(configSource);

        return new ConfigurationRoot(new List<IConfigurationProvider> { provider });
    }
}