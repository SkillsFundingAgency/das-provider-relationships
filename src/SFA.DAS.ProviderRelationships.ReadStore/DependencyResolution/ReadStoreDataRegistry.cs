using SFA.DAS.ProviderRelationships.ReadStore.Data;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.ReadStore.DependencyResolution
{
    internal class ReadStoreDataRegistry : Registry
    {
        public ReadStoreDataRegistry()
        {
            For<IDocumentClientFactory>().Use<DocumentClientFactory>().Singleton();
            For<IPermissionsRepository>().Use<PermissionsRepository>();
        }
    }
}