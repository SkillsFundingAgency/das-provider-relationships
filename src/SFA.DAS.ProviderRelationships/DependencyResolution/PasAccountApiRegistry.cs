using SFA.DAS.AutoConfiguration;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.PAS.Account.Api.Client;
using SFA.DAS.ProviderRelationships.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class PasAccountApiRegistry : Registry
    {
        public PasAccountApiRegistry()
        {
            For<IPasAccountApiConfiguration>().Use(c => c.GetInstance<IAutoConfigurationService>().Get<PasAccountApiConfiguration>()).Singleton();
            For<IPasAccountApiClient>().Use(c => new PasAccountApiClient(c.GetInstance<IAutoConfigurationService>().Get<ProviderRelationshipsConfiguration>().PasAccountApi));
        }
    }
}