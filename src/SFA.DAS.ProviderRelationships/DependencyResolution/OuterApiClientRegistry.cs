using SFA.DAS.ProviderRelationships.Services.OuterApi;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class OuterApiClientRegistry : Registry
    {
        public OuterApiClientRegistry()
        {
            For<IOuterApiClient>().Use<OuterApiClient>();
        }
    }
}