using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Messages.Events;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships
{
    public class AddedAccountProviderEventHandler : IHandleMessages<AddedAccountProviderEvent>
    {
        private readonly IMediator _mediator;

        public AddedAccountProviderEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(AddedAccountProviderEvent message, IMessageHandlerContext context)
        {
            return _mediator.Send(new AddedAccountProviderEventAuditCommand(
                message.AccountProviderId,
                message.AccountId,
                message.ProviderUkprn,
                message.UserRef,
                message.Added));
        }
    }
}
