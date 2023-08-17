using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Commands.AddAccountProvider
{
    public class AddAccountProviderCommandHandler : IRequestHandler<AddAccountProviderCommand, long>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly ILogger<AddAccountProviderCommandHandler> _logger;

        public AddAccountProviderCommandHandler(Lazy<ProviderRelationshipsDbContext> db, ILogger<AddAccountProviderCommandHandler> logger)
        {
            _db = db;
            _logger = logger;
        }
        
        public async Task<long> Handle(AddAccountProviderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{TypeName} starting processing of request: {Request}.", nameof(AddAccountProviderCommandHandler), JsonSerializer.Serialize(request));
            
            var account = await _db.Value.Accounts
                .Include(a => a.AccountProviders)
                .SingleAsync(a => a.Id == request.AccountId, cancellationToken);

            var provider = await _db.Value.Providers.SingleAsync(p => p.Ukprn == request.Ukprn, cancellationToken);

            _logger.LogInformation("{TypeName} Provider retrieved.", nameof(AddAccountProviderCommandHandler));
            
            var user = await _db.Value.Users.SingleAsync(u => u.Ref == request.UserRef, cancellationToken);
            
            _logger.LogInformation("{TypeName} User retrieved.", nameof(AddAccountProviderCommandHandler));
            
            var accountProvider = account.AddProvider(provider, user, request.CorrelationId);

            await _db.Value.SaveChangesAsync(cancellationToken);
            
            _logger.LogInformation("{TypeName} completed processing.", nameof(AddAccountProviderCommandHandler));
            
            return accountProvider.Id;
        }
    }
}