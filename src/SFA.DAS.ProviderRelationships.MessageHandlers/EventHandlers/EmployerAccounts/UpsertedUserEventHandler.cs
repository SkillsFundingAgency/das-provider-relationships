using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands.UpsertUser;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts
{
    public class UpsertedUserEventHandler : IHandleMessages<UpsertedUserEvent>
    {
        private readonly IMediator _mediator;

        public UpsertedUserEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(UpsertedUserEvent message, IMessageHandlerContext context)
        {
            return _mediator.Send(new UpsertUserCommand(message.UserRef, message.Created, "WILL_NEED_A_CORRELATIONID"));
        }
    }
}