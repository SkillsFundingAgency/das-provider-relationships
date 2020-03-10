using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAllProviders
{
    public class FindAllProvidersQueryResult
    {
        public List<ProviderDto> Providers { get; }

        public FindAllProvidersQueryResult(List<ProviderDto> providers)
        {
            Providers = providers;
        }
    }
}