using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRelationships.Data;
using Z.EntityFramework.Plus;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class DeleteAccountLegalEntityPermissionsCommandHandler : AsyncRequestHandler<DeleteAccountLegalEntityPermissionsCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public DeleteAccountLegalEntityPermissionsCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        protected override Task Handle(DeleteAccountLegalEntityPermissionsCommand request, CancellationToken cancellationToken)
        {
            return _db.Value.Permissions.Where(p => p.AccountLegalEntity.Id == request.AccountLegalEntityId).DeleteAsync(cancellationToken);
        }
    }
}