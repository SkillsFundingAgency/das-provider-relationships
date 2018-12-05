using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Audit.Commands;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts
{
    public class CreatedAccountEventHandler : IHandleMessages<CreatedAccountEvent>
    {
        private readonly IMediator _mediator;

        public CreatedAccountEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(CreatedAccountEvent message, IMessageHandlerContext context)
        {
            _mediator.Send(new CreatedAccountEventAuditCommand() {
                AccountId = message.AccountId,
                UserRef = message.UserRef,
                UserName = message.UserName,
                Name = message.Name,
                PublicHashedId = message.PublicHashedId
            });

	        return _mediator.Send(new CreateAccountCommand(message.AccountId, message.HashedId, message.PublicHashedId, message.Name, message.Created));

        }
    }
}