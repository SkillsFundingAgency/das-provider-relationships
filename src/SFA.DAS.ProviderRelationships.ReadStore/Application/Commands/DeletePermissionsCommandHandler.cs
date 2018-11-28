using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Commands
{
    internal class DeletePermissionsCommandHandler : IReadStoreRequestHandler<DeletePermissionsCommand, Unit>
    {
        private readonly IAccountProviderLegalEntitiesRepository _accountProviderLegalEntitiesRepository;

        public DeletePermissionsCommandHandler(IAccountProviderLegalEntitiesRepository accountProviderLegalEntitiesRepository)
        {
            _accountProviderLegalEntitiesRepository = accountProviderLegalEntitiesRepository;
        }
        
        public async Task<Unit> Handle(DeletePermissionsCommand request, CancellationToken cancellationToken)
        {
            var accountProviderLegalEntity = await _accountProviderLegalEntitiesRepository.CreateQuery().SingleAsync(r => 
                r.Ukprn == request.Ukprn && r.AccountProviderLegalEntityId == request.AccountProviderLegalEntityId, cancellationToken);
            
            accountProviderLegalEntity.Delete(request.Deleted, request.MessageId);
                
            await _accountProviderLegalEntitiesRepository.Update(accountProviderLegalEntity, null, cancellationToken);
            
            return Unit.Value;
        }
    }
}