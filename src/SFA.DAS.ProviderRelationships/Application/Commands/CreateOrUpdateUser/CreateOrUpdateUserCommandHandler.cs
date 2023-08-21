using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser;

public class CreateOrUpdateUserCommandHandler : IRequestHandler<CreateOrUpdateUserCommand>
{
    private readonly Lazy<ProviderRelationshipsDbContext> _db;
    private readonly ILogger<CreateOrUpdateUserCommandHandler> _logger;

    public CreateOrUpdateUserCommandHandler(Lazy<ProviderRelationshipsDbContext> db, ILogger<CreateOrUpdateUserCommandHandler> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Handle(CreateOrUpdateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{TypeName} started processing request.", nameof(CreateOrUpdateUserCommandHandler));
        
        var user = _db.Value.Users.SingleOrDefault(u => u.Ref == request.Ref);

        if (user == null)
        {
            _logger.LogInformation("{TypeName} adding user.", nameof(CreateOrUpdateUserCommandHandler));
            
           await _db.Value.Users.AddAsync(new User(request.Ref, request.Email, request.FirstName, request.LastName), cancellationToken);
        }
        else
        {
            _logger.LogInformation("{TypeName} updating user.", nameof(CreateOrUpdateUserCommandHandler));
            
            user.Update(request.Email, request.FirstName, request.LastName);
        }

        // Unit of Work is not persisting the changes to the DB via the AccountProvidersController.Invitation() method so this call ensures it does.
        // This may be due to the Invitation() method returning RedirectToAction() instead of returning a View() ?! ....
        await _db.Value.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("{TypeName} completed processing.", nameof(CreateOrUpdateUserCommandHandler));
    }
}