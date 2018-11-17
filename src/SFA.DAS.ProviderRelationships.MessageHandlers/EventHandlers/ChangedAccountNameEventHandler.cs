using System.Linq;
using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers
{
    public class ChangedAccountNameEventHandler : IHandleMessages<ChangedAccountNameEvent>
    {
        private readonly IMediator _mediator;

        public ChangedAccountNameEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(ChangedAccountNameEvent message, IMessageHandlerContext context)
        {
            var result = await _mediator.Send(new GetAccountProviderUkprnsQuery(message.AccountId)); 
            
            await Task.WhenAll(result.Ukprns.Select(u => context.SendLocal(new BatchUpdateRelationshipAccountNamesCommand(u, message.AccountId, message.Name, message.Created))));
        }
    }
}