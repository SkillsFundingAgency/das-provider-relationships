using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands.UpdateAccountLegalEntityName
{
    public class UpdateAccountLegalEntityNameCommand : IRequest
    {
        public long AccountLegalEntityId { get; }
        public string Name { get; }
        public DateTime Created { get; }

        public UpdateAccountLegalEntityNameCommand(long accountLegalEntityId, string name, DateTime created)
        {
            AccountLegalEntityId = accountLegalEntityId;
            Name = name;
            Created = created;
        }
    }
}