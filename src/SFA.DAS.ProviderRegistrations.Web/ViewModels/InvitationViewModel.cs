using System;

namespace SFA.DAS.ProviderRegistrations.Web.ViewModels
{
    public class InvitationViewModel
    {
        public string EmployerOrganisation { get; set; }

        public string EmployerFirstName { get; set; }

        public string EmployerLastName { get; set; }

        public string EmployerEmail { get; set; }

        public string Status { get; set; }

        public DateTime SentDate { get; set; }
    }
}
