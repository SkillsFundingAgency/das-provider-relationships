using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.CommandHandlers
{
    internal class BatchUpdateRelationshipAccountNamesCommandHandler : IHandleMessages<BatchUpdateRelationshipAccountNamesCommand>
    {
        private readonly IReadStoreMediator _mediator;

        public BatchUpdateRelationshipAccountNamesCommandHandler(IReadStoreMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(BatchUpdateRelationshipAccountNamesCommand message, IMessageHandlerContext context)
        {
            return _mediator.Send(message);
        }
    }
}