using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.CosmosDb;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.UpdatePermissions
{
    public class UpdatePermissionsCommandHandler : AsyncRequestHandler<UpdatePermissionsCommand>
    {
        private readonly IAccountProviderLegalEntitiesRepository _accountProviderLegalEntitiesRepository;
        private readonly ILog _log;

        public UpdatePermissionsCommandHandler(IAccountProviderLegalEntitiesRepository accountProviderLegalEntitiesRepository, ILog log)
        {
            _accountProviderLegalEntitiesRepository = accountProviderLegalEntitiesRepository;
            _log = log;
        }
        
        protected override async Task Handle(UpdatePermissionsCommand request, CancellationToken cancellationToken)
        {
            var accountProviderLegalEntity = await _accountProviderLegalEntitiesRepository.CreateQuery().SingleOrDefaultAsync(r => 
                r.Ukprn == request.Ukprn && r.AccountProviderLegalEntityId == request.AccountProviderLegalEntityId, cancellationToken);

            if (accountProviderLegalEntity == null)
            {
                try
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
                catch (Exception ex)
                {
                    _log.Error(ex, $"Failed to add Account Provider Legal Entity - AccountId={request.AccountId}, AccountLegalEntityId={request.AccountLegalEntityId}, AccountProviderId={request.AccountProviderId}, AccountProviderLegalEntityId={request.AccountProviderLegalEntityId}, Ukprn={request.Ukprn}");
                    throw;
                }
            }
            else
            {
                accountProviderLegalEntity.UpdatePermissions(request.GrantedOperations, request.Updated, request.MessageId);
                
                await _accountProviderLegalEntitiesRepository.Update(accountProviderLegalEntity, null, cancellationToken);
            }
        }
    }
}