using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.DeletePermissions;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships
{
#pragma warning disable 618
    public class DeletedPermissionsEventHandler : IHandleMessages<DeletedPermissionsEvent>
    {
        private readonly IMediator _mediator;

        public DeletedPermissionsEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(DeletedPermissionsEvent message, IMessageHandlerContext context)
        {
            return Task.WhenAll(
                _mediator.Send(new DeletedPermissionsEventAuditCommand(
                    message.AccountProviderLegalEntityId,
                    message.Ukprn,
                    message.Deleted)),
                _mediator.Send(new DeletePermissionsCommand(
                    message.AccountProviderLegalEntityId,
                    message.Ukprn,
                    message.Deleted,
                    context.MessageId)));
        }
    }
#pragma warning restore 618
}