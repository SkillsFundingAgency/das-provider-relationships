using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Audit.Commands;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships
{
    public class DeletedPermissionsEventHandler : IHandleMessages<DeletedPermissionsEvent>
    {
        private readonly IReadStoreMediator _readStoreMediator;
        private readonly IMediator _mediator;

        public DeletedPermissionsEventHandler(IReadStoreMediator readStoreMediator, IMediator mediator)
        {
            _readStoreMediator = readStoreMediator;
            _mediator = mediator;
        }

        public Task Handle(DeletedPermissionsEvent message, IMessageHandlerContext context)
        {
            return Task.WhenAll(
                _mediator.Send(new DeletedPermissionsEventAuditCommand(message.AccountProviderLegalEntityId,
                    message.Ukprn, message.Deleted)),

                _readStoreMediator.Send(new DeletePermissionsCommand(message.AccountProviderLegalEntityId,
                    message.Ukprn, message.Deleted, context.MessageId)));
        }
    }
}