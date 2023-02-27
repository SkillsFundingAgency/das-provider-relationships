using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateAccount;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;

public class CreatedAccountEventHandler : IHandleMessages<CreatedAccountEvent>
{
    private readonly IMediator _mediator;

    public CreatedAccountEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Handle(CreatedAccountEvent message, IMessageHandlerContext context)
    {
        return _mediator.Send(new CreateAccountCommand(message.AccountId, message.HashedId, message.PublicHashedId,
            message.Name, message.Created));
    }
}