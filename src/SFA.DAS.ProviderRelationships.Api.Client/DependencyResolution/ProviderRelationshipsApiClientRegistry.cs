﻿using System.Net.Http;
using Microsoft.Azure.Documents;
using MediatR;
using StructureMap;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;
using SFA.DAS.ProviderRelationships.Api.Client.Http;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasPermission;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Application.Queries.HasRelationshipWithPermission;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution
{
    public class ProviderRelationshipsApiClientRegistry : Registry
    {
        public ProviderRelationshipsApiClientRegistry()
        {
            IncludeRegistry<AutoConfigurationRegistry>();

            For<IDocumentClient>().Add(c => c.GetInstance<IDocumentClientFactory>().CreateDocumentClient()).Named(GetType().FullName).Singleton();
            For<IDocumentClientFactory>().Use<DocumentClientFactory>();
            For<IAccountProviderLegalEntitiesRepository>().Use<ReadOnlyAccountProviderLegalEntitiesRepository>().Ctor<IDocumentClient>().IsNamedInstance(GetType().FullName);

            For<IMediator>().Use<Mediator>();
            For<ServiceFactory>().Use<ServiceFactory>(ctx => ctx.GetInstance);

            For<IRequestHandler<HasPermissionQuery, bool>>().Use<HasPermissionQueryHandler>();
            For<IRequestHandler<HasRelationshipWithPermissionQuery, bool>>().Use<HasRelationshipWithPermissionQueryHandler>();

            //todo: move this into a pr comment?
            // can't get the scanning to work, but whilst scanning would make life more convenient for developers going forward,
            // individually registering handlers is probably more performant (at least for a reasonable number of handlers)
//            Scan(s =>
//            {
////                s.AssemblyContainingType<HasPermissionQueryHandler>();
//                s.TheCallingAssembly();
//                s.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
//                s.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
////                s.AddAllTypesOf(typeof(IRequestHandler<,>));
////                s.AddAllTypesOf(typeof(INotificationHandler<>));
//            });

            For<HttpClient>().Add(c => c.GetInstance<IHttpClientFactory>().CreateHttpClient()).Named(GetType().FullName).Singleton();
            For<IHttpClientFactory>().Use<Http.HttpClientFactory>();
            For<IRestHttpClient>().Use<RestHttpClient>().Ctor<HttpClient>().IsNamedInstance(GetType().FullName);
            For<IProviderRelationshipsApiClient>().Use<ProviderRelationshipsApiClient>();

            //todo: we can supply the rowkey for the new version, but it would be better to supply a version (instead of hardcoding DefaultVersion) and still pick up the row name from the type name
            // ^^ check with devops, but probably can't override the default version, but could tack the version number on the end of the name
            //todo: also use the config object in the other config classes
            
            For<ReadStoreConfiguration>().Use(c => ProviderRelationshipsApiClientConfiguration.Get(c).ReadStore);
            For<AzureAdClientConfiguration>().Use(c => ProviderRelationshipsApiClientConfiguration.Get(c).AzureAdClient);
        }
    }
}