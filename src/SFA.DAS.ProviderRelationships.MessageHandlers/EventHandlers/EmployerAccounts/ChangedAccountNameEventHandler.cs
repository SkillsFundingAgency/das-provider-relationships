using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands.UpdateAccountName;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;

public class ChangedAccountNameEventHandler : IHandleMessages<ChangedAccountNameEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<ChangedAccountNameEvent> _logger;

    public ChangedAccountNameEventHandler(IMediator mediator, ILogger<ChangedAccountNameEvent> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public Task Handle(ChangedAccountNameEvent message, IMessageHandlerContext context)
    {
        _logger.LogInformation("Starting {TypeName} handler.", nameof(ChangedAccountNameEventHandler));
        
        var result = _mediator.Send(new UpdateAccountNameCommand(message.AccountId, message.CurrentName, message.Created));
        
        _logger.LogInformation("Completed {TypeName} handler.", nameof(ChangedAccountNameEventHandler));

        return result;
    }
}