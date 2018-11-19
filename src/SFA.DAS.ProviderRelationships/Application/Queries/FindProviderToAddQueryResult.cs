namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class FindProviderToAddQueryResult
    {
        public long? Ukprn { get; }
        public long? AccountProviderId { get; }
        public bool ProviderNotFound => Ukprn == null;
        public bool ProviderAlreadyAdded => AccountProviderId != null;

        public FindProviderToAddQueryResult(long? ukprn, long? accountProviderId)
        {
            Ukprn = ukprn;
            AccountProviderId = accountProviderId;
        }
    }
}