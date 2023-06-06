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
    private readonly ILogger<UpdatePermissionsCommandHandler> _logger;

    public UpdatePermissionsCommandHandler(
        IAccountProviderLegalEntitiesRepository accountProviderLegalEntitiesRepository,
        ILogger<UpdatePermissionsCommandHandler> logger)
    {
        _accountProviderLegalEntitiesRepository = accountProviderLegalEntitiesRepository;
        _logger = logger;
    }

    public async Task Handle(UpdatePermissionsCommand request, CancellationToken cancellationToken)
    {
        var accountProviderLegalEntity = await _accountProviderLegalEntitiesRepository.CreateQuery()
            .SingleOrDefaultAsync(r =>
                    r.Ukprn == request.Ukprn && r.AccountProviderLegalEntityId == request.AccountProviderLegalEntityId,
                cancellationToken);

        _logger.LogInformation(
            "Starting {TypeName} for Command with Ukprn: {Ukprn} and AccountProviderLegalEntityId: {AccountProviderLegalEntityId}.",
            nameof(UpdatePermissionsCommandHandler),
            request.Ukprn, request.AccountProviderLegalEntityId);

        if (accountProviderLegalEntity == null)
        {
            _logger.LogInformation("AccountProviderLegalEntity not found in Readstore, adding ...");

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

                _logger.LogInformation("Adding to Readstore completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to add Account Provider Legal Entity - AccountId={AccountId}, AccountLegalEntityId={AccountLegalEntityId}, AccountProviderId={AccountProviderId}, AccountProviderLegalEntityId={AccountProviderLegalEntityId}, Ukprn={Ukprn}",
                    request.AccountId, request.AccountLegalEntityId, request.AccountProviderId,
                    request.AccountProviderLegalEntityId, request.Ukprn);
                throw;
            }
        }
        else
        {
            _logger.LogInformation("AccountProviderLegalEntity was found in Readstore, updating...");

            accountProviderLegalEntity.UpdatePermissions(request.GrantedOperations, request.Updated, request.MessageId);

            await _accountProviderLegalEntitiesRepository.Update(accountProviderLegalEntity, null, cancellationToken);

            _logger.LogInformation("Update of entity in Readstore completed successfully.");
        }
    }
}