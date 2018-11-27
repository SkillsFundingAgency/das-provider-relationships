using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Commands
{
    internal class UpdatePermissionsCommandHandler : IReadStoreRequestHandler<UpdatePermissionsCommand, Unit>
    {
        private readonly IRelationshipsRepository _relationshipsRepository;

        public UpdatePermissionsCommandHandler(IRelationshipsRepository relationshipsRepository)
        {
            _relationshipsRepository = relationshipsRepository;
        }
        
        public async Task<Unit> Handle(UpdatePermissionsCommand request, CancellationToken cancellationToken)
        {
            var relationship = await _relationshipsRepository.CreateQuery().SingleOrDefaultAsync(r => 
                r.Ukprn == request.Ukprn && r.AccountProviderLegalEntityId == request.AccountProviderLegalEntityId, cancellationToken);

            if (relationship == null)
            {
                relationship = new Relationship(
                    request.AccountId,
                    request.AccountLegalEntityId,
                    request.AccountProviderId,
                    request.AccountProviderLegalEntityId,
                    request.Ukprn,
                    request.GrantedOperations,
                    request.Updated,
                    request.MessageId);
                
                await _relationshipsRepository.Add(relationship, null, cancellationToken);
            }
            else
            {
                relationship.UpdatePermissions(request.GrantedOperations, request.Updated, request.MessageId);
                
                await _relationshipsRepository.Update(relationship, null, cancellationToken);
            }

            return Unit.Value;
        }
    }
}