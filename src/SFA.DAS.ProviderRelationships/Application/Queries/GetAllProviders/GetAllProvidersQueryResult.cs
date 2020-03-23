using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAllProviders
{
    public class GetAllProvidersQueryResult
    {
        public List<AccountProviderDto> Providers { get; }

        public GetAllProvidersQueryResult(List<AccountProviderDto> providers)
        {
            Providers = providers;
        }
    }
}