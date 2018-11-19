using System.Linq;
using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.EventHandlers
{
    public class DeletedAccountLegalEntityEventHandler : IHandleMessages<DeletedAccountLegalEntityEvent>
    {
        private readonly IMediator _mediator;

        public DeletedAccountLegalEntityEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(DeletedAccountLegalEntityEvent message, IMessageHandlerContext context)
        {
            var result = await _mediator.Send(new GetAccountProviderUkprnsByAccountIdQuery(message.AccountId));
         
            await _mediator.Send(new DeleteAccountLegalEntityPermissionsCommand(message.AccountLegalEntityId));   
            await Task.WhenAll(result.Ukprns.Select(u => context.SendLocal(new BatchDeleteRelationshipsCommand(u, message.AccountLegalEntityId, message.Deleted))));
        }
    }
}