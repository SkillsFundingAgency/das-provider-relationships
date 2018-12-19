using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class AddedAccountProviderEventAuditCommand : IRequest
    {
        public AddedAccountProviderEventAuditCommand(long accountProviderId, long accountId, long providerUkprn, Guid userRef, DateTime added)
        {
            AccountProviderId = accountProviderId;
            AccountId = accountId;
            ProviderUkprn = providerUkprn;
            UserRef = userRef;
            Added = added;
        }

        public long AccountProviderId { get; }
        public long AccountId { get; }
        public long ProviderUkprn { get; }
        public Guid UserRef { get; }
        public DateTime Added { get; }
    }
}