﻿using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.Providers.Api.Client;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class ApprenticeshipInfoServiceApiConfigurationRegistry : Registry
    {
        public ApprenticeshipInfoServiceApiConfigurationRegistry()
        {
            For<ApprenticeshipInfoServiceApiConfiguration>().Use(() => ConfigurationHelper.GetConfiguration<ApprenticeshipInfoServiceApiConfiguration>("SFA.DAS.ApprenticeshipInfoServiceAPI")).Singleton();
            For<IProviderApiClient>().Use(c => new ProviderApiClient(c.GetInstance<ApprenticeshipInfoServiceApiConfiguration>().BaseUrl));
        }
    }
}