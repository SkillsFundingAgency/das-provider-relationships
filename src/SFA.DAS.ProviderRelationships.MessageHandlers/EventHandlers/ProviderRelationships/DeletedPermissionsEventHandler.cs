using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.DeletePermissions;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;
#pragma warning disable 618
public class DeletedPermissionsEventHandler : IHandleMessages<DeletedPermissionsEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<DeletedPermissionsEventHandler> _logger;

    public DeletedPermissionsEventHandler(IMediator mediator, ILogger<DeletedPermissionsEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public Task Handle(DeletedPermissionsEvent message, IMessageHandlerContext context)
    {
        _logger.LogInformation("Starting {TypeName} handler.", nameof(DeletedPermissionsEventHandler));
        
        var result =  Task.WhenAll(
            _mediator.Send(new DeletedPermissionsEventAuditCommand(
                message.AccountProviderLegalEntityId,
                message.Ukprn,
                message.Deleted)),
            _mediator.Send(new DeletePermissionsCommand(
                message.AccountProviderLegalEntityId,
                message.Ukprn,
                message.Deleted,
                context.MessageId))
            );
        
        _logger.LogInformation("Completed {TypeName} handler.", nameof(DeletedPermissionsEventHandler));
        
        return result;
    }
}
#pragma warning restore 618