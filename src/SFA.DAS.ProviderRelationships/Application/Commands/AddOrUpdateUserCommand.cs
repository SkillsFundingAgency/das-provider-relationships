using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class AddOrUpdateUserCommand : IRequest
    {
        public Guid Ref { get; }
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public AddOrUpdateUserCommand(Guid @ref, string email, string firstName, string lastName)
        {
            Ref = @ref;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}