using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class RemoveAccountLegalEntityCommand : IRequest
    {
        public long AccountId { get; }
        public long AccountLegalEntityId { get; }
        public DateTime Created { get; }

        public RemoveAccountLegalEntityCommand(long accountId, long accountLegalEntityId, DateTime created)
        {
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            Created = created;
        }
    }
}