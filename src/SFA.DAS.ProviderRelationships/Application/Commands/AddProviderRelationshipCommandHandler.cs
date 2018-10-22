using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Exceptions;
using SFA.DAS.ProviderRelationships.Models;

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

            var accountLegalEntity = await db.AccountLegalEntities.FindAsync(request.AccountLegalEntityId);
            if (accountLegalEntity == null)  //todo: what exception to throw?
                throw new MissingEntityException($"Attempt to add a provider relationship to an unknown AccountLegalEntity (Id: {request.AccountLegalEntityId})");

            // providers will be added outside of these command handlers
            accountLegalEntity.AddRelationship(request.Ukprn);

            //todo: if provider relationship already exists, we need to surface that to the ui, so we can show message saying relationship already exists
        }
    }
}