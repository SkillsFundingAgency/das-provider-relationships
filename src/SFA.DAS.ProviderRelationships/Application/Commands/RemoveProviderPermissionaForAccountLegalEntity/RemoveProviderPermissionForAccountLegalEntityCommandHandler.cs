using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Commands.RemoveProviderPermissionsForAccountLegalEntity
{
    public class RemoveProviderPermissionsForAccountLegalEntityCommandHandler : AsyncRequestHandler<RemoveProviderPermissionsForAccountLegalEntityCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public RemoveProviderPermissionsForAccountLegalEntityCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        protected override async Task Handle(RemoveProviderPermissionsForAccountLegalEntityCommand request, CancellationToken cancellationToken)
        {
            var accountProviderLegalEntity = await _db.Value.AccountProviderLegalEntities
                .Include(x => x.AccountProvider)
                .Include(x => x.AccountLegalEntity)
                .Include(x => x.Permissions)
                .Where(x => x.AccountProvider.ProviderUkprn == request.Ukprn)
                .Where(x => x.AccountLegalEntity.PublicHashedId == request.AccountLegalEntityPublicHashedId)
                .SingleOrDefaultAsync();

            if (accountProviderLegalEntity == null)
                return;

            var remainingOperations = accountProviderLegalEntity
                .Permissions
                .Where(x => !request.OperationsToRemove.Contains(x.Operation))
                .Select(x => x.Operation);
            accountProviderLegalEntity.UpdatePermissions(
                user: null,
                grantedOperations: new HashSet<Types.Models.Operation>(remainingOperations));
        }
    }
}
