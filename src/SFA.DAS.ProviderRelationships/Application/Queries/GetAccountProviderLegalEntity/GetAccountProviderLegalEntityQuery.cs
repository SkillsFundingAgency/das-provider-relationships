using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity
{
    public class GetAccountProviderLegalEntityQuery : IRequest<GetAccountProviderLegalEntityQueryResult>
    {
        public long AccountId { get; }
        public long AccountProviderId { get; }
        public long AccountLegalEntityId { get; }

        public GetAccountProviderLegalEntityQuery(long accountId, long accountProviderId, long accountLegalEntityId)
        {
            AccountId = accountId;
            AccountProviderId = accountProviderId;
            AccountLegalEntityId = accountLegalEntityId;
        }
    }
}