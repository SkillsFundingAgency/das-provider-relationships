using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Domain.Data;

namespace SFA.DAS.ProviderRelationships.Application.Commands.RemoveAccountLegalEntity
{
    public class RemoveAccountLegalEntityCommandHandler : AsyncRequestHandler<RemoveAccountLegalEntityCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public RemoveAccountLegalEntityCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        protected override async Task Handle(RemoveAccountLegalEntityCommand request, CancellationToken cancellationToken)
        {
            var account = await _db.Value.Accounts.SingleAsync(a => a.Id == request.AccountId, cancellationToken);
            
            var accountLegalEntity = await _db.Value.AccountLegalEntities
                .IgnoreQueryFilters()
                .Include(ale => ale.AccountProviderLegalEntities)
                .ThenInclude(aple => aple.AccountProvider)
                .SingleAsync(ale => ale.Id == request.AccountLegalEntityId, cancellationToken);
            
            account.RemoveAccountLegalEntity(accountLegalEntity, request.Removed);
        }
    }
}