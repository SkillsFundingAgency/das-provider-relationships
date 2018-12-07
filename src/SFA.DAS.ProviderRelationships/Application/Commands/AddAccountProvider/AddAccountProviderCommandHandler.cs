using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Commands.AddAccountProvider
{
    public class AddAccountProviderCommandHandler : IRequestHandler<AddAccountProviderCommand, long>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public AddAccountProviderCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }
        
        public async Task<long> Handle(AddAccountProviderCommand request, CancellationToken cancellationToken)
        {
            var account = await _db.Value.Accounts.Include(a => a.AccountProviders).SingleAsync(a => a.Id == request.AccountId, cancellationToken);
            var provider = await _db.Value.Providers.SingleAsync(p => p.Ukprn == request.Ukprn, cancellationToken);
            var user = await _db.Value.Users.SingleAsync(u => u.Ref == request.UserRef, cancellationToken);
            var accountProvider = account.AddProvider(provider, user);

            await _db.Value.SaveChangesAsync(cancellationToken);
            
            return accountProvider.Id;
        }
    }
}