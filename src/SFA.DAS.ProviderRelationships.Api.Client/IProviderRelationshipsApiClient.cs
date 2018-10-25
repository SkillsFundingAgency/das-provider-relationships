﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public interface IProviderRelationshipsApiClient
    {
        Task<bool> HasRelationshipWithPermission(ProviderRelationshipRequest request);
        Task<ProviderRelationshipResponse> ListRelationshipsWithPermission(ProviderRelationshipRequest request);
    }
}