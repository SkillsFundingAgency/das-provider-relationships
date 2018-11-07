namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class SearchProvidersQueryReply
    {
        public long? Ukprn { get; }
        public int? AccountProviderId { get; }
        public bool ProviderNotFound => Ukprn == null;
        public bool ProviderAlreadyAdded => AccountProviderId != null;

        public SearchProvidersQueryReply(long? ukprn, int? accountProviderId)
        {
            Ukprn = ukprn;
            AccountProviderId = accountProviderId;
        }
    }
}