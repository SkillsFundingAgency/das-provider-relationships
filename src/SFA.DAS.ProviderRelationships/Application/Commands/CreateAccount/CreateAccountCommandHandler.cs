using System;
using MediatR;
using SFA.DAS.ProviderRelationships.Domain.Data;
using SFA.DAS.ProviderRelationships.Domain.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands.CreateAccount
{
    public class CreateAccountCommandHandler : RequestHandler<CreateAccountCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;

        public CreateAccountCommandHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
            _db = db;
        }

        protected override void Handle(CreateAccountCommand request)
        {
            var account = new Account(request.AccountId, request.HashedId, request.PublicHashedId, request.Name, request.Created);
            
            _db.Value.Accounts.Add(account);
        }
    }
}