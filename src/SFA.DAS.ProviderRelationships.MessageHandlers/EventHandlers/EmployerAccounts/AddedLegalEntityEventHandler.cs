using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands.AddAccountLegalEntity;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts;

public class AddedLegalEntityEventHandler : IHandleMessages<AddedLegalEntityEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<AddedLegalEntityEventHandler> _logger;

    public AddedLegalEntityEventHandler(IMediator mediator, ILogger<AddedLegalEntityEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(AddedLegalEntityEvent message, IMessageHandlerContext context)
    {
        _logger.LogInformation("Starting {TypeName} handler.", nameof(AddedLegalEntityEventHandler));

        await _mediator.Send(new AddAccountLegalEntityCommand(message.AccountId, message.AccountLegalEntityId,
            message.AccountLegalEntityPublicHashedId, message.OrganisationName, message.Created));

        _logger.LogInformation("Completed {TypeName} handler.", nameof(AddedLegalEntityEventHandler));
    }
}