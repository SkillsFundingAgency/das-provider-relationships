namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class SearchProvidersQueryResponse
    {
        public long? Ukprn { get; }
        public int? AccountProviderId { get; }

        public SearchProvidersQueryResponse(long? ukprn, int? accountProviderId)
        {
            Ukprn = ukprn;
            AccountProviderId = accountProviderId;
        }
    }
}