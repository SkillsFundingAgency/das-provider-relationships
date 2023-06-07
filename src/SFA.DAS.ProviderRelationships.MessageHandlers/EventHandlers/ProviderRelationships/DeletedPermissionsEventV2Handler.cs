using SFA.DAS.ProviderRelationships.Application.Commands.SendDeletedPermissionsNotification;
using SFA.DAS.ProviderRelationships.Messages.Events;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;

public class DeletedPermissionsEventV2Handler : IHandleMessages<DeletedPermissionsEventV2>
{
    private readonly IMediator _mediator;
    private readonly ILogger<DeletedPermissionsEventV2Handler> _logger;

    public DeletedPermissionsEventV2Handler(IMediator mediator, ILogger<DeletedPermissionsEventV2Handler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public Task Handle(DeletedPermissionsEventV2 message, IMessageHandlerContext context)
    {
        _logger.LogInformation("Starting {TypeName} handler.", nameof(DeletedPermissionsEventV2Handler));
        
        var command = new SendDeletedPermissionsNotificationCommand(message.Ukprn, message.AccountLegalEntityId);

        var result = _mediator.Send(command);

        _logger.LogInformation("Completed {TypeName} handler.", nameof(DeletedPermissionsEventV2Handler));

        return result;
    }
}