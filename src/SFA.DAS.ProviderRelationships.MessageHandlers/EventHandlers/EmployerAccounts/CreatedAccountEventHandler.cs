using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateAccount;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;

public class CreatedAccountEventHandler : IHandleMessages<CreatedAccountEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<CreatedAccountEventHandler> _logger;

    public CreatedAccountEventHandler(IMediator mediator, ILogger<CreatedAccountEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(CreatedAccountEvent message, IMessageHandlerContext context)
    {
        _logger.LogInformation("Starting {TypeName} handler.", nameof(CreatedAccountEventHandler));

        await _mediator.Send(new CreateAccountCommand(message.AccountId, message.HashedId, message.PublicHashedId,
            message.Name, message.Created));

        _logger.LogInformation("Completed {TypeName} handler.", nameof(CreatedAccountEventHandler));
    }
}