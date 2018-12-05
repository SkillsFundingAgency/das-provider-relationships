using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships
{
    public class UpdatedPermissionsEventHandler : IHandleMessages<UpdatedPermissionsEvent>
    {
        private readonly IReadStoreMediator _mediator;

        public UpdatedPermissionsEventHandler(IReadStoreMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(UpdatedPermissionsEvent message, IMessageHandlerContext context)
        {
            //todo audit

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