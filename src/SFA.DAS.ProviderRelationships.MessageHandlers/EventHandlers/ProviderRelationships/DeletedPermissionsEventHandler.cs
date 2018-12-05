using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships
{
    public class DeletedPermissionsEventHandler : IHandleMessages<DeletedPermissionsEvent>
    {
        private readonly IReadStoreMediator _mediator;

        public DeletedPermissionsEventHandler(IReadStoreMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(DeletedPermissionsEvent message, IMessageHandlerContext context)
        {
            //todo audit

            return _mediator.Send(new DeletePermissionsCommand(message.AccountProviderLegalEntityId, message.Ukprn, message.Deleted, context.MessageId));
        }
    }
}