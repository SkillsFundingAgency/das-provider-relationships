using SFA.DAS.ProviderRelationships.Api.Client.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.TestHarness.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {

            For<ProviderRelationshipsApiClientConfiguration>().Use(c => new ProviderRelationshipsApiClientConfiguration {
                ApiBaseUrl = "https://localhost:44308/",
                ClientId = "xxx",
                ClientSecret = "xxx",
                IdentifierUri = "https://citizenazuresfabisgov.onmicrosoft.com/xxx",
                Tenant = "citizenazuresfabisgov.onmicrosoft.com"
            });
            
            });
        }
    }
}