using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAllProviders
{
    public class GetAllProvidersQueryResult
    {
        public List<ProviderDto> Providers { get; }

        public GetAllProvidersQueryResult(List<ProviderDto> providers)
        {
            Providers = providers;
        }
    }
}