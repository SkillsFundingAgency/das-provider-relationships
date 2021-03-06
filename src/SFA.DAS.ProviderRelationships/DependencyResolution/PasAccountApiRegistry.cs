﻿using SFA.DAS.PAS.Account.Api.Client;
using SFA.DAS.ProviderRelationships.Configuration;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.DependencyResolution
{
    public class PasAccountApiRegistry : Registry
    {
        public PasAccountApiRegistry()
        {
            For<IPasAccountApiClient>().Use<PasAccountApiClient>();
            For<IPasAccountApiConfiguration>().Use(c => c.GetInstance<ProviderRelationshipsConfiguration>().PasAccountApi);
        }
    }
}