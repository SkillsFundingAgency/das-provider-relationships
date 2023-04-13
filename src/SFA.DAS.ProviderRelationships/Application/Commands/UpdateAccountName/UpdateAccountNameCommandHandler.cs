using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ProviderRelationships.Data;

namespace SFA.DAS.ProviderRelationships.Application.Commands.UpdateAccountName;

public class UpdateAccountNameCommandHandler : IRequestHandler<UpdateAccountNameCommand>
{
    private readonly Lazy<ProviderRelationshipsDbContext> _db;

    public UpdateAccountNameCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
    {
        _db = db;
    }

    public async Task Handle(UpdateAccountNameCommand request, CancellationToken cancellationToken)
    {
        var account = await _db.Value.Accounts.SingleAsync(a => a.Id == request.AccountId, cancellationToken);

        account.UpdateName(request.Name, request.Created);
    }
}