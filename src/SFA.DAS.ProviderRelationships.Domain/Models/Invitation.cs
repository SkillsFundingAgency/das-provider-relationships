using System;

namespace SFA.DAS.ProviderRelationships.Domain.Models
{
    public class Invitation : Entity
    {
        public int Id { get; }

        public Guid Reference { get; }

        public string Ukprn { get; }

        public string UserRef { get; }

        public string EmployerOrganisation { get; }

        public string EmployerFirstName { get; }

        public string EmployerLastName { get; }

        public string EmployerEmail { get; }

        public int Status { get; }

        public DateTime CreatedDate { get; }

        public DateTime UpdatedDate { get; }

        public Invitation(Guid reference, string ukprn, string userRef, string organisation, string firstName, string lastName, string email, int status, DateTime createdDate)
        {
            Reference = reference;
            Ukprn = ukprn;
            UserRef = userRef;
            EmployerOrganisation = organisation;
            EmployerFirstName = firstName;
            EmployerLastName = lastName;
            EmployerEmail = email;
            Status = status;
            CreatedDate = createdDate;
            UpdatedDate = createdDate;
        }
    }
}
