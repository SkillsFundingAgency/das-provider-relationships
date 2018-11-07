using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetProviderQueryReply
    {
        public ProviderDto Provider { get; }

        public GetProviderQueryReply(ProviderDto provider)
        {
            Provider = provider;
        }
    }
}