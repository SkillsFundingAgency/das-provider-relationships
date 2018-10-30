﻿using SFA.DAS.ProviderRelationships.Api.Client.Application;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb;
using SFA.DAS.ProviderRelationships.Document.Repository.DependencyResolution;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.DependencyResolution
{
    public class ProviderRelationshipsApiClientRegistry : Registry
    {
        public ProviderRelationshipsApiClientRegistry()
        {
            IncludeRegistry<DocumentRegistry>();

            For<IProviderRelationshipService>().Use<ProviderRelationshipService>();
            For<IProviderRelationshipsApiClient>().Use<ProviderRelationshipsApiClient>();

            // TODO config to wire up later
            For<IDocumentConfiguration>().Use(new CosmosDbConfiguration
            {
                DatabaseName = "SFA",
                Uri = "https://localhost:8081",
                SecurityKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
            });

            For(typeof(IDocumentRepository<>)).Use(typeof(DocumentRepository<>)).Ctor<string>()
                .Is("provider-relationships");
            For(typeof(IDocumentReadOnlyRepository<>)).Use(typeof(DocumentReadOnlyRepository<>)).Ctor<string>()
                .Is("provider-relationships");
        }
    }
}