using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.UpdatePermissions;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships
{
    public class UpdatedPermissionsEventHandler : IHandleMessages<UpdatedPermissionsEvent>
    {
        private readonly IMediator _mediator;

        public UpdatedPermissionsEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(UpdatedPermissionsEvent message, IMessageHandlerContext context)
        {
            return _mediator.Send(new UpdatePermissionsCommand(
                message.AccountId,
                message.AccountLegalEntityId,
                message.AccountProviderId,
                message.AccountProviderLegalEntityId,
                message.Ukprn,
                message.GrantedOperations,
                message.Updated,
                context.MessageId));
        }
    }
}