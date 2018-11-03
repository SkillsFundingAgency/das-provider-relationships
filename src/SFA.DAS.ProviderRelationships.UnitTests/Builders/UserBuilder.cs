using System;
using Moq;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.UnitTests.Builders
{
    public class UserBuilder
    {
        private readonly Mock<User> _user = new Mock<User> { CallBase = true };

        public UserBuilder WithRef(Guid @ref)
        {
            _user.SetupProperty(u => u.Ref, @ref);

            return this;
        }

        public UserBuilder WithEmail(string email)
        {
            _user.SetupProperty(u => u.Email, email);

            return this;
        }

        public UserBuilder WithFirstName(string firstName)
        {
            _user.SetupProperty(u => u.FirstName, firstName);

            return this;
        }

        public UserBuilder WithLastName(string lastName)
        {
            _user.SetupProperty(u => u.LastName, lastName);

            return this;
        }
        
        public User Build()
        {
            return _user.Object;
        }

        public UserBuilder WithCreated(DateTime created)
        {
            _user.SetupProperty(u => u.Created, created);
            
            return this;
        }
    }
}