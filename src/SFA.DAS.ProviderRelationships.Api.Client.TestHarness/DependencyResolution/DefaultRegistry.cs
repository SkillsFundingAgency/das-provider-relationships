﻿using SFA.DAS.ProviderRelationships.Api.Client.Configuration;
using SFA.DAS.ProviderRelationships.ReadStore.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.Client.TestHarness.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<ProviderRelationshipsApiClientConfiguration>().Use(() => new ProviderRelationshipsApiClientConfiguration
            {
                ApiBaseUrl = "https://localhost:44308",
                ClientId = "xxx",
                ClientSecret = "xxx",
                IdentifierUri = "https://citizenazuresfabisgov.onmicrosoft.com/xxx",
                Tenant = "citizenazuresfabisgov.onmicrosoft.com"
            });
            
            For<ProviderRelationshipsReadStoreConfiguration>().Use(() => new ProviderRelationshipsReadStoreConfiguration
            {
                Uri = "https://localhost:8081",
                AuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
            });
        }
    }
}