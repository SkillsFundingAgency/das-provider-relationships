namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class SearchProvidersQueryResponse
    {
        public long? Ukprn { get; }
        public int? AccountProviderId { get; }
        public bool ProviderFound => Ukprn != null;
        public bool ProviderAlreadyAdded => AccountProviderId != null;

        public SearchProvidersQueryResponse(long? ukprn, int? accountProviderId)
        {
            Ukprn = ukprn;
            AccountProviderId = accountProviderId;
        }
    }
}