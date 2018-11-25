using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetUpdatedAccountProviderLegalEntityQuery : IRequest<GetUpdatedAccountProviderLegalEntityQueryResult>
    {
        public long AccountId { get; }
        public long AccountProviderLegalEntityId { get; }

        public GetUpdatedAccountProviderLegalEntityQuery(long accountId, long accountProviderLegalEntityId)
        {
            AccountId = accountId;
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
        }
    }
}