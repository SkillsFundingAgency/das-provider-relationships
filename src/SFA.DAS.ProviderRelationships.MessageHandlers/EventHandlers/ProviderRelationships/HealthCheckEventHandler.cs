using SFA.DAS.ProviderRelationships.Application.Commands.ReceiveProviderRelationshipsHealthCheckEvent;
using SFA.DAS.ProviderRelationships.Messages.Events;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.ProviderRelationships;

public class HealthCheckEventHandler : IHandleMessages<HealthCheckEvent>
{
    private readonly IMediator _mediator;

    public HealthCheckEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Handle(HealthCheckEvent message, IMessageHandlerContext context)
    {
        return _mediator.Send(new ReceiveProviderRelationshipsHealthCheckEventCommand(message.Id));
    }
}