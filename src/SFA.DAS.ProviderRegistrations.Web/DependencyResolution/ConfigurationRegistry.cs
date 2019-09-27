using Microsoft.Extensions.Configuration;
using SFA.DAS.Authorization.ProviderFeatures.Configuration;
using SFA.DAS.Encoding;
using SFA.DAS.ProviderRegistrations.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRegistrations.Web.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            //AddConfiguration<ApprenticeshipInfoServiceConfiguration>(ProviderRegistrationsConfigurationKeys.ApprenticeshipInfoServiceConfiguration);
            AddConfiguration<AuthenticationSettings>(ProviderRegistrationsConfigurationKeys.AuthenticationSettings);
            //AddConfiguration<CommitmentsClientApiConfiguration>(ProviderCommitmentsConfigurationKeys.CommitmentsClientApiConfiguration);
            //AddConfiguration<CommitmentPermissionsApiClientConfiguration>(ProviderCommitmentsConfigurationKeys.CommitmentsClientApiConfiguration);
            AddConfiguration<EncodingConfig>(ProviderRegistrationsConfigurationKeys.Encoding);
            AddConfiguration<ProviderFeaturesConfiguration>(ProviderRegistrationsConfigurationKeys.FeaturesConfiguration);
            AddConfiguration<ProviderRegistrationsSettings>(ProviderRegistrationsConfigurationKeys.ProviderRegistrations);
        }

        private void AddConfiguration<T>(string key) where T : class
        {
            For<T>().Use(c => GetConfiguration<T>(c, key)).Singleton();
        }

        private T GetConfiguration<T>(IContext context, string name)
        {
            var configuration = context.GetInstance<IConfiguration>();
            var section = configuration.GetSection(name);
            var value = section.Get<T>();

            return value;
        }
    }
}