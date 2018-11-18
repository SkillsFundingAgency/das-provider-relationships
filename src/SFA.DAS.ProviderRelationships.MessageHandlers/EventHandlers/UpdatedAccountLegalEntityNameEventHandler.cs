using System.Linq;
using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers
{
    public class UpdatedAccountLegalEntityNameEventHandler : IHandleMessages<UpdatedAccountLegalEntityNameEvent>
    {
        private readonly IMediator _mediator;

        public UpdatedAccountLegalEntityNameEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(UpdatedAccountLegalEntityNameEvent message, IMessageHandlerContext context)
        {
            var result = await _mediator.Send(new GetAccountProviderUkprnsByAccountIdQuery(message.AccountId));
            
            await Task.WhenAll(result.Ukprns.Select(u => context.SendLocal(new BatchUpdateRelationshipAccountLegalEntityNamesCommand(u, message.AccountLegalEntityId, message.Name, message.Created))));
        }
    }
}