using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Commands
{
    internal class BatchUpdateRelationshipAccountNamesCommandHandler : IReadStoreRequestHandler<BatchUpdateRelationshipAccountNamesCommand, Unit>
    {
        public Task<Unit> Handle(BatchUpdateRelationshipAccountNamesCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Unit.Value);
        }
    }
}