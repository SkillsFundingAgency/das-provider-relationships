using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProviderLegalEntity
{
    public class GetAccountProviderLegalEntityQuery : IRequest<GetAccountProviderLegalEntityQueryResult>
    {
        public long AccountId { get; }
        public long AccountProviderId { get; }
        public long AccountLegalEntityId { get; }
        public string AccountHashedId { get; internal set; }

        public GetAccountProviderLegalEntityQuery(string employerAccountHashedId, long accountId, long accountProviderId, long accountLegalEntityId)
        {
            AccountHashedId = employerAccountHashedId;
            AccountId = accountId;
            AccountProviderId = accountProviderId;
            AccountLegalEntityId = accountLegalEntityId;
        }
    }
}