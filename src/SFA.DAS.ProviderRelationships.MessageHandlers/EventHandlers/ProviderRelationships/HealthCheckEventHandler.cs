using SFA.DAS.ProviderRelationships.Application.Commands.ReceiveProviderRelationshipsHealthCheckEvent;
using SFA.DAS.ProviderRelationships.Messages.Events;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;

public class HealthCheckEventHandler : IHandleMessages<HealthCheckEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<HealthCheckEventHandler> _logger;

    public HealthCheckEventHandler(IMediator mediator, ILogger<HealthCheckEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public Task Handle(HealthCheckEvent message, IMessageHandlerContext context)
    {
        _logger.LogInformation("Starting {TypeName} handler.", nameof(HealthCheckEventHandler));
        
        var result = _mediator.Send(new ReceiveProviderRelationshipsHealthCheckEventCommand(message.Id));
        
        _logger.LogInformation("Completed {TypeName} handler.", nameof(HealthCheckEventHandler));

        return result;
    }
}