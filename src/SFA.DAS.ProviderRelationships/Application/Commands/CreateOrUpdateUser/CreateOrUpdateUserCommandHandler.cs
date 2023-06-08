using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser;

public class CreateOrUpdateUserCommandHandler : IRequestHandler<CreateOrUpdateUserCommand>
{
    private readonly Lazy<ProviderRelationshipsDbContext> _db;

    public CreateOrUpdateUserCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
    {
        _db = db;
    }

    public async Task Handle(CreateOrUpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _db.Value.Users.SingleOrDefault(u => u.Ref == request.Ref);

        if (user == null)
        {
           await _db.Value.Users.AddAsync(new User(request.Ref, request.Email, request.FirstName, request.LastName), cancellationToken);
        }
        else
        {
            user.Update(request.Email, request.FirstName, request.LastName);
        }

        await _db.Value.SaveChangesAsync(cancellationToken);
    }
}