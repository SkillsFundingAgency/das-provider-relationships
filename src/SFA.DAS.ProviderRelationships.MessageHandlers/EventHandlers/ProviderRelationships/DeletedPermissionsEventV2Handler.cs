using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Application.Commands.SendDeletedPermissionsNotification;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.DeletePermissions;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships
{
    public class DeletedPermissionsEventV2Handler : IHandleMessages<DeletedPermissionsEventV2>
    {
        private readonly IMediator _mediator;

        public DeletedPermissionsEventV2Handler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(DeletedPermissionsEventV2 message, IMessageHandlerContext context)
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
                    context.MessageId)),
                _mediator.Send(new SendDeletedPermissionsNotificationCommand(
                    message.Ukprn,
                    message.AccountLegalEntityId)));
        }
    }
}