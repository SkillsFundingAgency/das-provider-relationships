using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.Providers.Api.Client;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class ApprenticeshipInfoServiceApiRegistry : Registry
    {
        public ApprenticeshipInfoServiceApiRegistry()
        {
            For<ApprenticeshipInfoServiceApiConfiguration>().Use(c => c.GetInstance<IConfiguration>().Get<ApprenticeshipInfoServiceApiConfiguration>("SFA.DAS.ApprenticeshipInfoServiceAPI")).Singleton();
            For<IProviderApiClient>().Use(c => new ProviderApiClient(c.GetInstance<ApprenticeshipInfoServiceApiConfiguration>().BaseUrl));
        }
    }
}