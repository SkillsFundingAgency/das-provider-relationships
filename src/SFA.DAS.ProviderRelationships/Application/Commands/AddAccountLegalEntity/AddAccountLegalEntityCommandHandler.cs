using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Commands.AddAccountLegalEntity;

public class AddAccountLegalEntityCommandHandler : IRequestHandler<AddAccountLegalEntityCommand>
{
    private readonly Lazy<ProviderRelationshipsDbContext> _db;

    public AddAccountLegalEntityCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
    {
        _db = db;
    }

    public async Task Handle(AddAccountLegalEntityCommand request, CancellationToken cancellationToken)
    {
        var account = await _db.Value.Accounts.SingleAsync(a => a.Id == request.AccountId, cancellationToken);
            
        account.AddAccountLegalEntity(request.AccountLegalEntityId, request.AccountLegalEntityPublicHashedId, request.OrganisationName, request.Created);
    }
}