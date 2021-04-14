using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity
{
    public class GetAccountProviderLegalEntityQuery : IRequest<GetAccountProviderLegalEntityQueryResult>
    {
        public string AccountHashedId { get; }
        public long AccountId { get; }
        public long AccountProviderId { get; }
        public long AccountLegalEntityId { get; }

        public GetAccountProviderLegalEntityQuery(string accountHashedId, long accountId, long accountProviderId, long accountLegalEntityId)
        {
            AccountHashedId = accountHashedId;
            AccountId = accountId;
            AccountProviderId = accountProviderId;
            AccountLegalEntityId = accountLegalEntityId;
        }
    }
}