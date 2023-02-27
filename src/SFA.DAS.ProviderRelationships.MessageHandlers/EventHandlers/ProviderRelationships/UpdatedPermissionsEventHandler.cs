using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Application.Commands.SendUpdatedPermissionsNotification;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.UpdatePermissions;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;

public class UpdatedPermissionsEventHandler : IHandleMessages<UpdatedPermissionsEvent>
{
    private readonly IMediator _mediator;

    public UpdatedPermissionsEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(UpdatedPermissionsEvent message, IMessageHandlerContext context)
    {
        var tasks = Task.WhenAll(
            _mediator.Send(new UpdatedPermissionsEventAuditCommand(
                message.AccountId,
                message.AccountLegalEntityId,
                message.AccountProviderId,
                message.AccountProviderLegalEntityId,
                message.Ukprn,
                message.UserRef,
                message.GrantedOperations,
                message.Updated)),
            _mediator.Send(new UpdatePermissionsCommand(
                message.AccountId,
                message.AccountLegalEntityId,
                message.AccountProviderId,
                message.AccountProviderLegalEntityId,
                message.Ukprn,
                message.GrantedOperations,
                message.Updated,
                context.MessageId)));

        tasks.Wait();

        if (tasks.Status == TaskStatus.RanToCompletion)
        {
            await _mediator.Send(new SendUpdatedPermissionsNotificationCommand(
                message.Ukprn,
                message.AccountLegalEntityId,
                message.PreviousOperations,
                message.GrantedOperations));
        }
    }
}