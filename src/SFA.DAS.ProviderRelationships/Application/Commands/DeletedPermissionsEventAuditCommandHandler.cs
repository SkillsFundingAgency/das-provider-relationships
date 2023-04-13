using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands;

public class DeletedPermissionsEventAuditCommandHandler : IRequestHandler<DeletedPermissionsEventAuditCommand>
{
    private readonly Lazy<ProviderRelationshipsDbContext> _db;

    public DeletedPermissionsEventAuditCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
    {
        _db = db;
    }

    public async Task Handle(DeletedPermissionsEventAuditCommand request, CancellationToken cancellationToken)
    {
        var audit = new DeletedPermissionsEventAudit(request.AccountProviderLegalEntityId, request.Ukprn, request.Deleted);

        await _db.Value.DeletedPermissionsEventAudits.AddAsync(audit, cancellationToken);
    }
}