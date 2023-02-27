using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands.UpdateAccountName;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;

public class ChangedAccountNameEventHandler : IHandleMessages<ChangedAccountNameEvent>
{
    private readonly IMediator _mediator;

    public ChangedAccountNameEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Handle(ChangedAccountNameEvent message, IMessageHandlerContext context)
    {
        return _mediator.Send(new UpdateAccountNameCommand(message.AccountId, message.CurrentName, message.Created));
    }
}