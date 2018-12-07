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
            _mediator.Send(new DeletedPermissionsEventAuditCommand {
                AccountProviderLegalEntityId = message.AccountProviderLegalEntityId,
                Ukprn = message.Ukprn,
                Deleted = message.Deleted
            });

            return _readStoreMediator.Send(new DeletePermissionsCommand(message.AccountProviderLegalEntityId, message.Ukprn, message.Deleted, context.MessageId));
        }
    }
}