using Microsoft.Azure.Documents;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Queries;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution
{
    public class ProviderRelationshipsApiClientRegistry : Registry
    {
        public ProviderRelationshipsApiClientRegistry()
        {
            For<ApiServiceFactory>().Use<ApiServiceFactory>(c => c.GetInstance);
            For<IApiMediator>().Use<ApiMediator>();
            For<IApiRequestHandler<GetRelationshipWithPermissionQuery, GetRelationshipWithPermissionQueryResult>>().Use<GetRelationshipWithPermissionQueryHandler>();
            For<IApiRequestHandler<HasRelationshipWithPermissionQuery, bool>>().Use<HasRelationshipWithPermissionQueryHandler>();
            For<IApiRequestHandler<HasRelationshipWithPermissionQuery, bool>>().Use<HasRelationshipWithPermissionQueryHandler>();
            For<IDocumentDbClient>().Add(c => c.GetInstance<IDocumentClientFactory>().CreateDocumentDbClient()).Named(GetType().FullName).Singleton();
            For<IDocumentClientFactory>().Use<DocumentClientFactory>();
            For<IPermissionsRepository>().Use<PermissionsRepository>().Ctor<IDocumentDbClient>().IsNamedInstance(GetType().FullName);
            For<IProviderRelationshipsApiClient>().Use<ProviderRelationshipsApiClient>();
        }
    }
}