using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetUpdatedAccountProviderLegalEntityQuery : IRequest<GetUpdatedAccountProviderLegalEntityQueryResult>
    {
        public long AccountId { get; }
        public long AccountProviderId { get; }
        public long AccountLegalEntityId { get; }

        public GetUpdatedAccountProviderLegalEntityQuery(long accountId, long accountProviderId, long accountLegalEntityId)
        {
            AccountId = accountId;
            AccountProviderId = accountProviderId;
            AccountLegalEntityId = accountLegalEntityId;
        }
    }
}