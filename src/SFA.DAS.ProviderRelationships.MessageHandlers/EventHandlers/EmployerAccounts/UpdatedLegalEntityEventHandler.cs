using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands.UpdateAccountLegalEntityName;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;

public class UpdatedLegalEntityEventHandler : IHandleMessages<UpdatedLegalEntityEvent>
{
    private readonly IMediator _mediator;

    public UpdatedLegalEntityEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Handle(UpdatedLegalEntityEvent message, IMessageHandlerContext context)
    {
        return _mediator.Send(new UpdateAccountLegalEntityNameCommand(message.AccountLegalEntityId, message.Name, message.Created));
    }
}