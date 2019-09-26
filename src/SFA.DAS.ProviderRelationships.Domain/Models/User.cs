using System;

namespace SFA.DAS.ProviderRelationships.Domain.Models
{
    public class User : Entity
    {
        public Guid Ref { get; private set; }
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Updated { get; private set; }

        public User(Guid @ref, string email, string firstName, string lastName)
        {
            Ref = @ref;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Created = DateTime.UtcNow;
        }

        private User()
        {
        }

        public void Update(string email, string firstName, string lastName)
        {
            if (email != Email || firstName != FirstName || lastName != LastName)
            {
                Email = email;
                FirstName = firstName;
                LastName = lastName;
                Updated = DateTime.UtcNow;
            }
        }

        public HealthCheck CreateHealthCheck()
        {
            return new HealthCheck(this);
        }
    }
}