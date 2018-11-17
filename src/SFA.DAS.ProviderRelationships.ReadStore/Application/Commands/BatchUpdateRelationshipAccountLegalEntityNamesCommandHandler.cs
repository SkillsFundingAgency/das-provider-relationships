using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Commands
{
    internal class BatchUpdateRelationshipAccountLegalEntityNamesCommandHandler : IReadStoreRequestHandler<BatchUpdateRelationshipAccountLegalEntityNamesCommand, Unit>
    {
        public Task<Unit> Handle(BatchUpdateRelationshipAccountLegalEntityNamesCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Unit.Value);
        }
    }
}