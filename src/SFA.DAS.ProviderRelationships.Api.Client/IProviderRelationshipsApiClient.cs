﻿using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public interface IProviderRelationshipsApiClient
    {
        Task<bool> HasRelationshipWithPermission(ProviderRelationshipsRequest request, CancellationToken cancellationToken = default);
        Task<ProviderRelationshipsResponse> GetRelationshipsWithPermission(ProviderRelationshipsRequest request, CancellationToken cancellationToken = default);
    }
}