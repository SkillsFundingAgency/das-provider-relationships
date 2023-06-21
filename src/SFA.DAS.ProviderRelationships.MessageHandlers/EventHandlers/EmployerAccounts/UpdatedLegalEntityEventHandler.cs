using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands.UpdateAccountLegalEntityName;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;

public class UpdatedLegalEntityEventHandler : IHandleMessages<UpdatedLegalEntityEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<UpdatedLegalEntityEventHandler> _logger;

    public UpdatedLegalEntityEventHandler(IMediator mediator, ILogger<UpdatedLegalEntityEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(UpdatedLegalEntityEvent message, IMessageHandlerContext context)
    {
        _logger.LogInformation("Starting {TypeName} handler.", nameof(UpdatedLegalEntityEventHandler));
        
        await _mediator.Send(new UpdateAccountLegalEntityNameCommand(message.AccountLegalEntityId, message.Name, message.Created));
        
        _logger.LogInformation("Completed {TypeName} handler.", nameof(UpdatedLegalEntityEventHandler));
    }
}