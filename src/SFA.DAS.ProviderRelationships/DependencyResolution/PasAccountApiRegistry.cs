using SFA.DAS.PAS.Account.Api.ClientV2;
using SFA.DAS.PAS.Account.Api.ClientV2.Configuration;
using SFA.DAS.ProviderRelationships.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class PasAccountApiRegistry : Registry
    {
        public PasAccountApiRegistry()
        {
            For<IPasAccountApiClient>().Use<PasAccountApiClient>();
            For<PasAccountApiConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>().PasAccountApi);
        }
    }
}