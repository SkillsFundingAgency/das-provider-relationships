using System.Linq;
using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers
{
    public class UpdatedAccountNameEventHandler : IHandleMessages<UpdatedAccountNameEvent>
    {
        private readonly IMediator _mediator;

        public UpdatedAccountNameEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(UpdatedAccountNameEvent message, IMessageHandlerContext context)
        {
            var result = await _mediator.Send(new GetAccountProviderUkprnsByAccountIdQuery(message.AccountId));
            
            await Task.WhenAll(result.Ukprns.Select(u => context.SendLocal(new BatchUpdateRelationshipAccountNamesCommand(u, message.AccountId, message.Name, message.Updated))));
        }
    }
}