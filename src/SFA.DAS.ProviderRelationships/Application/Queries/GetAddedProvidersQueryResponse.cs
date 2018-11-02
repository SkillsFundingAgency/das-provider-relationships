using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAddedProvidersQueryResponse
    {
        public IEnumerable<AccountProviderDto> AccountProviders { get; }

        public GetAddedProvidersQueryResponse(IEnumerable<AccountProviderDto> accountProviders)
        {
            AccountProviders = accountProviders;
        }
    }
}