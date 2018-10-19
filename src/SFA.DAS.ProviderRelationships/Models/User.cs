using System;

namespace SFA.DAS.ProviderRelationships.Models
{
    public class User
    {
        public virtual Guid Ref { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual string FirstName { get; protected set; }
        public virtual string LastName { get; protected set; }
        public virtual DateTime Created { get; protected set; }
        public virtual DateTime? Updated { get; protected set; }

        public User(Guid @ref, string email, string firstName, string lastName)
        {
            Ref = @ref;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Created = DateTime.UtcNow;
        }

        protected User()
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