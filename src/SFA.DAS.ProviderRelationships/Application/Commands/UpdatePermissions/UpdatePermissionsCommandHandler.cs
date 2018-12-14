using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Commands.UpdatePermissions
{
    public class UpdatePermissionsCommandHandler : AsyncRequestHandler<UpdatePermissionsCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public UpdatePermissionsCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }
        
        protected override async Task Handle(UpdatePermissionsCommand request, CancellationToken cancellationToken)
        {   
            var accountProvider = await _db.Value.AccountProviders.SingleAsync(ap => ap.Id == request.AccountProviderId && ap.AccountId == request.AccountId, cancellationToken);
            await _db.Value.AccountProviderLegalEntities.Include(aple => aple.Permissions).Where(aple => aple.AccountLegalEntityId == request.AccountLegalEntityId).LoadAsync(cancellationToken);
            var accountLegalEntity = await _db.Value.AccountLegalEntities.SingleAsync(ale => ale.Id == request.AccountLegalEntityId && ale.AccountId == request.AccountId, cancellationToken);
            var user = await _db.Value.Users.SingleAsync(u => u.Ref == request.UserRef, cancellationToken);
            
            accountProvider.UpdatePermissions(accountLegalEntity, user, request.GrantedOperations);
        }
    }
}