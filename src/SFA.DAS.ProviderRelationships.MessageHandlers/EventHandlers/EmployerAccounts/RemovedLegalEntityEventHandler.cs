using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands.RemoveAccountLegalEntity;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;

public class RemovedLegalEntityEventHandler : IHandleMessages<RemovedLegalEntityEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<RemovedLegalEntityEventHandler> _logger;

    public RemovedLegalEntityEventHandler(IMediator mediator, ILogger<RemovedLegalEntityEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(RemovedLegalEntityEvent message, IMessageHandlerContext context)
    {
        _logger.LogInformation("Starting {TypeName} handler for accountId: '{AccountId}'.", nameof(RemovedLegalEntityEventHandler), message.AccountId);
        
        await _mediator.Send(new RemoveAccountLegalEntityCommand(message.AccountId, message.AccountLegalEntityId, message.Created));
        
        _logger.LogInformation("Completed {TypeName} handler.", nameof(RemovedLegalEntityEventHandler));
    }
}