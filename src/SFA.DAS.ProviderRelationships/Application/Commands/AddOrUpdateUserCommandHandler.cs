using System;
using System.Linq;
using MediatR;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class AddOrUpdateUserCommandHandler : RequestHandler<AddOrUpdateUserCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public AddOrUpdateUserCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        protected override void Handle(AddOrUpdateUserCommand request)
        {
            var user = _db.Value.Users.SingleOrDefault(u => u.Ref == request.Ref);

            if (user == null)
            {
                _db.Value.Users.Add(new User(request.Ref, request.Email, request.FirstName, request.LastName));
            }
            else
            {
                user.Update(request.Email, request.FirstName, request.LastName);
            }
        }
    }
}