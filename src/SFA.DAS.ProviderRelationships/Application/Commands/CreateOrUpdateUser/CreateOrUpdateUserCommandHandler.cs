using System;
using System.Linq;
using MediatR;
using SFA.DAS.ProviderRelationships.Configuration;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Services;

namespace SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser
{
    public class CreateOrUpdateUserCommandHandler : RequestHandler<CreateOrUpdateUserCommand>
    {
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private readonly IOuterApiClient _outerApiClient;
        private readonly ProviderRelationshipsConfiguration _config;

        public CreateOrUpdateUserCommandHandler(
            Lazy<ProviderRelationshipsDbContext> db, 
            IOuterApiClient outerApiClient,
            ProviderRelationshipsConfiguration config)
        {
            _db = db;
            _outerApiClient = outerApiClient;
            _config = config;
        }

        protected override void Handle(CreateOrUpdateUserCommand command)
        {
            if (_config.UseGovUkSignIn.HasValue && _config.UseGovUkSignIn.Value)
            {
                var request = new GetEmployerAccountRequest(command.Ref.ToString(), command.Email);
                _outerApiClient.Get<GetUserAccountsResponse>(request);
            }
            else
            {
                var user = _db.Value.Users.SingleOrDefault(u => u.Ref == command.Ref);

                if (user == null)
                {
                    _db.Value.Users.Add(new User(command.Ref, command.Email, command.FirstName, command.LastName));
                }
                else
                {
                    user.Update(command.Email, command.FirstName, command.LastName);
                }                
            }
        }
    }
}