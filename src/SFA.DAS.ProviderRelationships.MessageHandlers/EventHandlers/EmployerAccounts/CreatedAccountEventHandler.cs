using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.ProviderRelationships.Application.Commands.CreateAccount;
using SFA.DAS.ProviderRelationships.Application.Commands.CreatedAccountEventStatistic;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers.EmployerAccounts
{
    public class CreatedAccountEventHandler : IHandleMessages<CreatedAccountEvent>
    {
        private readonly IMediator _mediator;

        public CreatedAccountEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(CreatedAccountEvent message, IMessageHandlerContext context)
        {
            return Task.WhenAll(
                _mediator.Send(new CreateAccountCommand(message.AccountId, message.HashedId, message.PublicHashedId, message.Name, message.Created)),
                _mediator.Send(new CreatedAccountEventStatisticCommand(message.AccountId, message.HashedId, message.PublicHashedId, message.Name, message.Created)));
        }
    }
}