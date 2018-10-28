namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class SearchProvidersQueryResponse
    {
        public long Ukprn { get; }
        public bool ProviderExists { get; }

        public SearchProvidersQueryResponse(long ukprn, bool providerExists)
        {
            Ukprn = ukprn;
            ProviderExists = providerExists;
        }
    }
}