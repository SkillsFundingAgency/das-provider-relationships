﻿using SFA.DAS.AutoConfiguration;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.Providers.Api.Client;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class ApprenticeshipInfoServiceApiRegistry : Registry
    {
        public ApprenticeshipInfoServiceApiRegistry()
        {
            IncludeRegistry<AutoConfigurationRegistry>();
            For<ApprenticeshipInfoServiceApiConfiguration>().Use(c => c.GetInstance<IAutoConfigurationService>().Get<ApprenticeshipInfoServiceApiConfiguration>(ConfigurationKeys.ApprenticeshipInfoServiceApi)).Singleton();
            For<IProviderApiClient>().Use(c => new ProviderApiClient(c.GetInstance<ApprenticeshipInfoServiceApiConfiguration>().BaseUrl));
        }
    }
}