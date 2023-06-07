using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Messages.Events;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;

public class AddedAccountProviderEventHandler : IHandleMessages<AddedAccountProviderEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<AddedAccountProviderEventHandler> _logger;

    public AddedAccountProviderEventHandler(IMediator mediator, ILogger<AddedAccountProviderEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(AddedAccountProviderEvent message, IMessageHandlerContext context)
    {
        _logger.LogInformation("Starting {TypeName} handler.", nameof(AddedAccountProviderEventHandler));
        
        await _mediator.Send(new AddedAccountProviderEventAuditCommand(
            message.AccountProviderId,
            message.AccountId,
            message.ProviderUkprn,
            message.UserRef,
            message.Added));
        
        _logger.LogInformation("Completed {TypeName} handler.", nameof(AddedAccountProviderEventHandler));
    }
}