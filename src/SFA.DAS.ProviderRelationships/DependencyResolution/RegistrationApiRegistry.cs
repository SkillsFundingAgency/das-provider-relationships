using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Services;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class RegistrationApiRegistry : Registry
    {
        public RegistrationApiRegistry()
        {
            For<RegistrationApiConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>().RegistrationApiClientConfiguration);
            For<IRegistrationApiHttpClientFactory>().Use<RegistrationApiHttpClientFactory>();
            For<IRegistrationService>().Use<RegistrationService>();
        }
    }
}
