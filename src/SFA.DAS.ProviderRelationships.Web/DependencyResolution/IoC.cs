using System;
using StructureMap;
using SFA.DAS.Authorization;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.DependencyResolution;
using SFA.DAS.ProviderRelationships.Web.Controllers;
using SFA.DAS.UnitOfWork.EntityFrameworkCore;
using SFA.DAS.UnitOfWork.NServiceBus;
using SFA.DAS.UnitOfWork.NServiceBus.ClientOutbox;

namespace SFA.DAS.ProviderRelationships.Web.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            var container = new Container(c =>
            {
                c.AddRegistry<ApprenticeshipInfoServiceApiRegistry>();
                c.AddRegistry<AuthorizationRegistry>();
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<DataRegistry>();
                c.AddRegistry<EmployerFeaturesAuthorizationRegistry>();
                c.AddRegistry<EmployerUserRolesAuthorizationRegistry>();
                c.AddRegistry<EntityFrameworkCoreUnitOfWorkRegistry<ProviderRelationshipsDbContext>>();
                c.AddRegistry<HashingRegistry>();
                c.AddRegistry<LoggerRegistry>();
                c.AddRegistry<MapperRegistry>();
                c.AddRegistry<MediatorRegistry>();
                c.AddRegistry<NServiceBusClientUnitOfWorkRegistry>();
                c.AddRegistry<NServiceBusUnitOfWorkRegistry>();
                c.AddRegistry<OidcAuthenticationRegistry>();
                c.AddRegistry<ProviderRelationshipsApiClientRegistry>();
                c.AddRegistry<StartupRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });

            var whatIHave = container.WhatDoIHave();

            try
            {
                var controller = container.GetInstance<AccountProvidersController>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return container;
        }
    }
}