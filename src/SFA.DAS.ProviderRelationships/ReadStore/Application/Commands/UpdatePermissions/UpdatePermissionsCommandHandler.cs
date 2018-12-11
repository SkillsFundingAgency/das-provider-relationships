using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.UpdatePermissions
{
    public class UpdatePermissionsCommandHandler : IReadStoreRequestHandler<UpdatePermissionsCommand, Unit>
    {
        private readonly IAccountProviderLegalEntitiesRepository _accountProviderLegalEntitiesRepository;

        public UpdatePermissionsCommandHandler(IAccountProviderLegalEntitiesRepository accountProviderLegalEntitiesRepository)
        {
            _accountProviderLegalEntitiesRepository = accountProviderLegalEntitiesRepository;
        }
        
        public async Task<Unit> Handle(UpdatePermissionsCommand request, CancellationToken cancellationToken)
        {
            var accountProviderLegalEntity = await _accountProviderLegalEntitiesRepository.CreateQuery().SingleOrDefaultAsync(r => 
                r.Ukprn == request.Ukprn && r.AccountProviderLegalEntityId == request.AccountProviderLegalEntityId, cancellationToken);

            if (accountProviderLegalEntity == null)
            {
                accountProviderLegalEntity = new AccountProviderLegalEntity(
                    request.AccountId,
                    request.AccountLegalEntityId,
                    request.AccountProviderId,
                    request.AccountProviderLegalEntityId,
                    request.Ukprn,
                    request.GrantedOperations,
                    request.Updated,
                    request.MessageId);
                
                await _accountProviderLegalEntitiesRepository.Add(accountProviderLegalEntity, null, cancellationToken);
            }
            else
            {
                accountProviderLegalEntity.UpdatePermissions(request.GrantedOperations, request.Updated, request.MessageId);
                
                await _accountProviderLegalEntitiesRepository.Update(accountProviderLegalEntity, null, cancellationToken);
            }

            return Unit.Value;
        }
    }
}