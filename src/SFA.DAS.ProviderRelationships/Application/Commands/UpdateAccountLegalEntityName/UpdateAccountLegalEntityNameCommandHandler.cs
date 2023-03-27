using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Commands.UpdateAccountLegalEntityName;

public class UpdateAccountLegalEntityNameCommandHandler : IRequestHandler<UpdateAccountLegalEntityNameCommand>
{
    private readonly Lazy<ProviderRelationshipsDbContext> _db;

    public UpdateAccountLegalEntityNameCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
    {
        _db = db;
    }

    public async Task Handle(UpdateAccountLegalEntityNameCommand request, CancellationToken cancellationToken)
    {
        var accountLegalEntity = await _db.Value.AccountLegalEntities.IgnoreQueryFilters().SingleAsync(a => a.Id == request.AccountLegalEntityId, cancellationToken);

        accountLegalEntity.UpdateName(request.Name, request.Created);
    }
}