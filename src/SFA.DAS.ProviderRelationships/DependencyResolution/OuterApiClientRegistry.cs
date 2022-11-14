using System.Net.Http;
using SFA.DAS.ProviderRelationships.Services.OuterApi;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class OuterApiClientRegistry : Registry
    {
        public OuterApiClientRegistry()
        {
            For<HttpClient>().Use<HttpClient>().SelectConstructor(() => new HttpClient());
            For<IOuterApiClient>().Use<OuterApiClient>();
        }
    }
}