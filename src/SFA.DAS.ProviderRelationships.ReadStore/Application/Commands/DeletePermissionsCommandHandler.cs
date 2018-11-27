using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Commands
{
    internal class DeletePermissionsCommandHandler : IReadStoreRequestHandler<DeletePermissionsCommand, Unit>
    {
        private readonly IRelationshipsRepository _relationshipsRepository;

        public DeletePermissionsCommandHandler(IRelationshipsRepository relationshipsRepository)
        {
            _relationshipsRepository = relationshipsRepository;
        }
        
        public async Task<Unit> Handle(DeletePermissionsCommand request, CancellationToken cancellationToken)
        {
            var relationship = await _relationshipsRepository.CreateQuery().SingleAsync(r => 
                r.Ukprn == request.Ukprn && r.AccountProviderLegalEntityId == request.AccountProviderLegalEntityId, cancellationToken);
            
            relationship.Delete(request.Deleted, request.MessageId);
                
            await _relationshipsRepository.Update(relationship, null, cancellationToken);
            
            return Unit.Value;
        }
    }
}