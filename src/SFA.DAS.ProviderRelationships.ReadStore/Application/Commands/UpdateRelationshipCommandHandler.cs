using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.CosmosDb;
using SFA.DAS.ProviderRelationships.ReadStore.Data;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using SFA.DAS.ProviderRelationships.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Application.Commands
{
    internal class UpdateRelationshipCommandHandler : IReadStoreRequestHandler<UpdateRelationshipCommand, Unit>
    {
        private readonly IRelationshipsRepository _relationshipsRepository;

        public UpdateRelationshipCommandHandler(IRelationshipsRepository relationshipsRepository)
        {
            _relationshipsRepository = relationshipsRepository;
        }
        public async Task<Unit> Handle(UpdateRelationshipCommand request, CancellationToken cancellationToken)
        {
            var relationship = await _relationshipsRepository.CreateQuery()
                .SingleOrDefaultAsync(x => x.Ukprn == request.Ukprn &&
                                           x.AccountProviderId == request.AccountProviderId &&
                                           x.AccountLegalEntityId == request.AccountLegalEntityId, cancellationToken);

            if (relationship == null)
            {
                relationship = new Relationship(request.Ukprn, request.AccountProviderId, request.AccountId, request.AccountLegalEntityId,
                    request.Operations, request.MessageId, request.Updated);
                await _relationshipsRepository.Add(relationship, null, cancellationToken);
            }
            else
            {
                relationship.UpdatePermissions(request.Operations, request.Updated, request.MessageId);
                await _relationshipsRepository.Update(relationship, null, cancellationToken);
            }

            return Unit.Value;
        }
    }
}