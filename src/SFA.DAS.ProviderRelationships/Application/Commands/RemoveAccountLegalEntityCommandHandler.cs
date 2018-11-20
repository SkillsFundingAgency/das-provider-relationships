using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Commands
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
            var accountLegalEntity = await _db.Value.AccountLegalEntities.SingleAsync(ale => ale.Id == request.AccountLegalEntityId, cancellationToken);
            
            account.RemoveAccountLegalEntity(accountLegalEntity, request.Created);
        }
    }
}