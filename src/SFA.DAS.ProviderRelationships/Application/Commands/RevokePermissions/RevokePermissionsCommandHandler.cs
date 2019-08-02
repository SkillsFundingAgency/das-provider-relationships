using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Encoding;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Commands.RevokePermissions
{
    public class RevokePermissionsCommandHandler : AsyncRequestHandler<RevokePermissionsCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IEncodingService _encodingService;

        public RevokePermissionsCommandHandler(Lazy<ProviderRelationshipsDbContext> db, IEncodingService encodingService)
        {
            _db = db;
            _encodingService = encodingService;
        }

        protected override async Task Handle(RevokePermissionsCommand command, CancellationToken cancellationToken)
        {
            long id;
            if (!_encodingService.TryDecode(command.AccountLegalEntityPublicHashedId, EncodingType.AccountLegalEntityId, out id))
                return;

            var accountProviderLegalEntity = await _db.Value.AccountProviderLegalEntities
                .Include(x => x.AccountProvider)
                .Include(x => x.AccountLegalEntity)
                .Include(x => x.Permissions)
                .Where(x => x.AccountProvider.ProviderUkprn == command.Ukprn)
                .Where(x => x.AccountLegalEntity.Id == id)
                .SingleOrDefaultAsync(cancellationToken);

            if (accountProviderLegalEntity == null)
                return;

            accountProviderLegalEntity.RevokePermissions(
                user: null,
                operationsToRevoke: command.OperationsToRevoke);
        }
    }
}
