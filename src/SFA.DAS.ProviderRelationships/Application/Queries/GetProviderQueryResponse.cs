using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetProviderQueryResponse
    {
        public ProviderDto Provider { get; }

        public GetProviderQueryResponse(ProviderDto provider)
        {
            Provider = provider;
        }
    }
}