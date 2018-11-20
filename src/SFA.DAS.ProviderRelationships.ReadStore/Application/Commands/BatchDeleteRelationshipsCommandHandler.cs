using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Commands
{
    internal class BatchDeleteRelationshipsCommandHandler : IReadStoreRequestHandler<BatchDeleteRelationshipsCommand, Unit>
    {
        public Task<Unit> Handle(BatchDeleteRelationshipsCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Unit.Value);
        }
    }
}