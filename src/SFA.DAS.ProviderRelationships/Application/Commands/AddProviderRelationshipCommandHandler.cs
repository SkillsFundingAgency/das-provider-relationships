using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class AddProviderRelationshipCommandHandler : AsyncRequestHandler<AddProviderRelationshipCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public AddProviderRelationshipCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        protected override async Task Handle(AddProviderRelationshipCommand request, CancellationToken cancellationToken)
        {
            var db = _db.Value;

            var accountLegalEntity = await db.AccountLegalEntities.SingleAsync(a => a.Id == request.AccountLegalEntityId, cancellationToken);

            // providers will be added outside of these command handlers
            accountLegalEntity.AddRelationship(request.Ukprn);

            //todo: if provider relationship already exists, we need to surface that to the ui, so we can show message saying relationship already exists
        }
    }
}