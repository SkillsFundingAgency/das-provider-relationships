using SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd
{
    public class GetProviderToAddQueryResult
    {
        public ProviderDto Provider { get; }

        public GetProviderToAddQueryResult(ProviderDto provider)
        {
            Provider = provider;
        }
    }
}