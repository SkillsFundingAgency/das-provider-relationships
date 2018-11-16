using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetProviderQueryResult
    {
        public ProviderDto Provider { get; }

        public GetProviderQueryResult(ProviderDto provider)
        {
            Provider = provider;
        }
    }
}