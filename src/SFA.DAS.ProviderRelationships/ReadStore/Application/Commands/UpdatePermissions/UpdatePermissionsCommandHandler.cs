using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Commands.UpdatePermissions;

public class UpdatePermissionsCommandHandler : IRequestHandler<UpdatePermissionsCommand>
{
    private readonly IAccountProviderLegalEntitiesRepository _accountProviderLegalEntitiesRepository;
    private readonly ILogger<UpdatePermissionsCommandHandler> _log;

    public UpdatePermissionsCommandHandler(IAccountProviderLegalEntitiesRepository accountProviderLegalEntitiesRepository, ILogger<UpdatePermissionsCommandHandler> log)
    {
        _accountProviderLegalEntitiesRepository = accountProviderLegalEntitiesRepository;
        _log = log;
    }

    public async Task Handle(UpdatePermissionsCommand request, CancellationToken cancellationToken)
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
                _log.LogError(ex, "Failed to add Account Provider Legal Entity - AccountId={AccountId}, AccountLegalEntityId={AccountLegalEntityId}, AccountProviderId={AccountProviderId}, AccountProviderLegalEntityId={AccountProviderLegalEntityId}, Ukprn={Ukprn}",
                    request.AccountId, request.AccountLegalEntityId, request.AccountProviderId, request.AccountProviderLegalEntityId, request.Ukprn);
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