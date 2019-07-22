using SFA.DAS.ProviderRelationships.Api.Client.Http;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution.StructureMap
{
    internal class HttpRegistry : Registry
    {
        public HttpRegistry()
        {
            For<IProviderRelationshipsApiClient>().Use(c => c.GetInstance<IProviderRelationshipsApiClientFactory>().CreateApiClient()).Singleton();
            For<IProviderRelationshipsApiClientFactory>().Use<ProviderRelationshipsApiClientFactory>().Transient();
        }
    }
}