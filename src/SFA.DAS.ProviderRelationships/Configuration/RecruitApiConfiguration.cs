﻿using SFA.DAS.Http.Configuration;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    public class RecruitApiConfiguration : IManagedIdentityClientConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string IdentifierUri { get; set; }
    }
}