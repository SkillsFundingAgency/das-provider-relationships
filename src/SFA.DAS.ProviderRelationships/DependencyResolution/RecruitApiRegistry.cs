using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Services;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class RecruitApiRegistry : Registry
    {
        public RecruitApiRegistry()
        {
            For<RecruitApiConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>().RecruitApiClientConfiguration);
            For<IRecruitApiHttpClientFactory>().Use<RecruitApiHttpClientFactory>();
            For<IDasRecruitService>().Use<DasRecruitService>();
        }
    }
}
