using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands.CreateOrUpdateUser
{
    public class CreateOrUpdateUserCommand : IRequest
    {
        public Guid Ref { get; }
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public CreateOrUpdateUserCommand(Guid @ref, string email, string firstName, string lastName)
        {
            Ref = @ref;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}