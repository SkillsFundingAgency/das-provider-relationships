using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Services;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class RoatpApiRegistry : Registry
    {
        public RoatpApiRegistry ()
        {
            For<RoatpApiConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>().RoatpApiClientSettings);
            For<IRoatpApiHttpClientFactory>().Use<RoatpApiHttpClientFactory>();
            For<IRoatpService>().Use<RoatpService>();
        }
    }
}