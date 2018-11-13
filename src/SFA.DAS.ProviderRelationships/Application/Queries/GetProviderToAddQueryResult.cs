using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
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