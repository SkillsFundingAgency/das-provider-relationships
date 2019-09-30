using System;

namespace SFA.DAS.ProviderRelationships.Domain.Models
{
    public class Invitation : Entity
    {
        public long Id { get; set; }

        public Guid Reference { get; }

        public string Ukprn { get; }

        public string UserRef { get; }

        public string EmployerOrganisation { get; }

        public string EmployerFirstName { get; }

        public string EmployerLastName { get; }

        public string EmployerEmail { get; }

        public string ProviderEmail { get; }

        public int Status { get; }

        public DateTime CreatedDate { get; }

        public DateTime UpdatedDate { get; }

        public Invitation(Guid reference, string ukprn, string userRef, string employerOrganisation, string employerFirstName, string employerLastName, string employerEmail, string providerEmail, int status, DateTime createdDate, DateTime updatedDate)
        {
            Reference = reference;
            Ukprn = ukprn;
            UserRef = userRef;
            EmployerOrganisation = employerOrganisation;
            EmployerFirstName = employerFirstName;
            EmployerLastName = employerLastName;
            EmployerEmail = employerEmail;
            ProviderEmail = providerEmail;
            Status = status;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
        }
    }
}
