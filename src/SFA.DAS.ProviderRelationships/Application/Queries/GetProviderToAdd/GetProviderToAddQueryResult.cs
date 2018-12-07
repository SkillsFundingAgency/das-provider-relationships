using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd
{
    public class GetProviderToAddQueryResult
    {
        public ProviderBasicDto Provider { get; }

        public GetProviderToAddQueryResult(ProviderBasicDto provider)
        {
            Provider = provider;
        }
    }
}