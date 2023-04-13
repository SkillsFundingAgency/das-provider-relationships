using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands;

public class UpdatedPermissionsEventAuditCommandHandler : IRequestHandler<UpdatedPermissionsEventAuditCommand>
{
    private readonly Lazy<ProviderRelationshipsDbContext> _db;

    public UpdatedPermissionsEventAuditCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
    {
        _db = db;
    }

    public async Task Handle(UpdatedPermissionsEventAuditCommand request, CancellationToken cancellationToken)
    {
        var audit = new UpdatedPermissionsEventAudit(
            request.AccountId,
            request.AccountLegalEntityId,
            request.AccountProviderId,
            request.AccountProviderLegalEntityId,
            request.Ukprn,
            request.UserRef,
            request.GrantedOperations,
            request.Updated);
            
        await _db.Value.UpdatedPermissionsEventAudits.AddAsync(audit, cancellationToken);
    }
}