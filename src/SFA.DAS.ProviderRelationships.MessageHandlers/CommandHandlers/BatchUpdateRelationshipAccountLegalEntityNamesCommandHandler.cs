using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.MessageHandlers.CommandHandlers
{
    internal class BatchUpdateRelationshipAccountLegalEntityNamesCommandHandler : IHandleMessages<BatchUpdateRelationshipAccountLegalEntityNamesCommand>
    {
        private readonly IReadStoreMediator _mediator;

        public BatchUpdateRelationshipAccountLegalEntityNamesCommandHandler(IReadStoreMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(BatchUpdateRelationshipAccountLegalEntityNamesCommand message, IMessageHandlerContext context)
        {
            return _mediator.Send(message);
        }
    }
}