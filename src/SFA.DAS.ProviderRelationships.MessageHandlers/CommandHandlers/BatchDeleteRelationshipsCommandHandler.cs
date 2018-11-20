using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.CommandHandlers
{
    internal class BatchDeleteRelationshipsCommandHandler : IHandleMessages<BatchDeleteRelationshipsCommand>
    {
        private readonly IReadStoreMediator _mediator;

        public BatchDeleteRelationshipsCommandHandler(IReadStoreMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(BatchDeleteRelationshipsCommand message, IMessageHandlerContext context)
        {
            return _mediator.Send(message);
        }
    }
}