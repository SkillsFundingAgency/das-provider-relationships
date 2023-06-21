using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands.CreateAccount;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand>
{
    private readonly Lazy<ProviderRelationshipsDbContext> _db;

    public CreateAccountCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
    {
        _db = db;
    }

    public async Task Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = new Account(request.AccountId, request.HashedId, request.PublicHashedId, request.Name, request.Created);

        await _db.Value.Accounts.AddAsync(account, cancellationToken);
    }
}