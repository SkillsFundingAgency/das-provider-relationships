using System;

namespace SFA.DAS.ProviderRelationships.Domain.Models
{
    public class Invitation : Entity
    {
        public long Id { get; set; }

        public Guid Reference { get; private set; }

        public long Ukprn { get; private set; }

        public string UserRef { get; private set; }

        public string EmployerOrganisation { get; private set; }

        public string EmployerFirstName { get; private set; }

        public string EmployerLastName { get; private set; }

        public string EmployerEmail { get; private set; }

        public int Status { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime UpdatedDate { get; private set; }

        public Invitation(Guid reference, long ukprn, string userRef, string employerOrganisation, string employerFirstName, string employerLastName, string employerEmail, int status, DateTime createdDate, DateTime updatedDate)
        {
            Reference = reference;
            Ukprn = ukprn;
            UserRef = userRef;
            EmployerOrganisation = employerOrganisation;
            EmployerFirstName = employerFirstName;
            EmployerLastName = employerLastName;
            EmployerEmail = employerEmail;
            Status = status;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
        }

        private Invitation()
        {
        }

        public void UpdateStatus(int status, DateTime updated)
        {
            Status = status;
            UpdatedDate = updated;
        }
    }
}
