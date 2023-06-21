using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands;

public class AddedAccountProviderEventAuditCommandHandler : IRequestHandler<AddedAccountProviderEventAuditCommand>
{
    private readonly Lazy<ProviderRelationshipsDbContext> _db;

    public AddedAccountProviderEventAuditCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
    {
        _db = db;
    }

    public async Task Handle(AddedAccountProviderEventAuditCommand request, CancellationToken cancellationToken)
    {
        var audit = new AddedAccountProviderEventAudit(request.AccountProviderId, request.AccountId, request.ProviderUkprn, request.UserRef, request.Added);
            
        await _db.Value.AddedAccountProviderEventAudits.AddAsync(audit, cancellationToken);
    }
}