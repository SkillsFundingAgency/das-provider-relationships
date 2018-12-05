using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Audit.Commands
{
    public class CreatedAccountEventAuditCommand : IRequest
    {
        public long AccountId { get; set; }
        public string PublicHashedId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public Guid UserRef { get; set; }
    }
}
