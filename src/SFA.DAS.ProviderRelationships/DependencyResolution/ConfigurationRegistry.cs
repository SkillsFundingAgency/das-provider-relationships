﻿using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.Encoding;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;
using SFA.DAS.ProviderRelationships.Configuration;
using StructureMap;
using ConfigurationKeys = SFA.DAS.ProviderRelationships.Configuration.ConfigurationKeys;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            IncludeRegistry<AutoConfigurationRegistry>();
            For<EmployerFeaturesConfiguration>().Use(c => c.GetInstance<IAutoConfigurationService>().Get<EmployerFeaturesConfiguration>(ConfigurationKeys.EmployerFeatures)).Singleton();
            For<IAzureActiveDirectoryConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>().AzureActiveDirectory).Singleton();
            For<IEmployerUrlsConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>().EmployerUrls).Singleton();
            For<IOidcConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>().Oidc).Singleton();
            For<GovUkOidcConfiguration>().Use(c => c.GetInstance<IAutoConfigurationService>().Get<GovSignIn>(ConfigurationKeys.GovUkSignin).GovUkOidcConfiguration).Singleton();
            For<ProviderRelationshipsConfiguration>().Use(c => c.GetInstance<IAutoConfigurationService>().Get<ProviderRelationshipsConfiguration>(ConfigurationKeys.ProviderRelationships)).Singleton();
            For<ReadStoreConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>().ReadStore).Singleton();
            For<ProviderRelationshipsApiConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>().ProviderRelationshipsApi);
            For<EncodingConfig>().Use(c => c.GetInstance<IAutoConfigurationService>().Get<EncodingConfig>(ConfigurationKeys.EncodingConfig)).Singleton();
            For<OuterApiConfiguration>().Use(c => c.GetInstance<OuterApiConfiguration>()).Singleton();
        }
    }
}