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

            //todo: provider may already exist?
            
            //todo: add ale, then add relationship where provider already exists. check join created
            
            //do we want to check existence?
            var accountLegalEntity = await db.AccountLegalEntities.FindAsync(request.AccountLegalEntityId);
            if (accountLegalEntity == null)  //todo: what exception to throw?
                throw new MissingEntityException($"Attempt to add a provider relationship to an unknown AccountLegalEntity (Id: {request.AccountLegalEntityId})");

            //todo: add Provider instance into command??
            accountLegalEntity.AddRelationship(new Provider(request.Ukprn, request.ProviderName, DateTime.UtcNow));
            
            //todo:
//            // if provider already exists, we don't need the name
//            // if provider already exists need to surface to ui so can show message relationship already exists
            // provider exists, or relationships exists?

            
            //            var provider = await db.Providers.FindAsync(request.Ukprn);
//            if (provider == null)
//                provider = db.Providers.Add(new Provider(request.Ukprn, request.ProviderName, DateTime.UtcNow)).Entity;
//
//            var accountLegalEntityProvider =
//                new AccountLegalEntityProvider(request.AccountLegalEntityId, request.Ukprn);
//            
//            // go through entity? (AddLegalEntityProvider, or AddProviderRelationship/AddAccountLegalEntityRelationship?)
//            // a way to enforce adding both? where would that belong though? on AccountLegalEntityProviders?
//            // doesn't know anything about dbcontext and probably a good thing
//            accountLegalEntity.AccountLegalEntityProviders.Add(accountLegalEntityProvider);
//            //provider.AccountLegalEntityProviders.Add(accountLegalEntityProvider);
//            
//            //looks like only have to add rel to 1, so could get away with not finding ale?
        }
    }
}