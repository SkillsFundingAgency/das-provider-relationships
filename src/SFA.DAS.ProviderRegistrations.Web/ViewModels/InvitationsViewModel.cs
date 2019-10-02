using System.Collections.Generic;

namespace SFA.DAS.ProviderRegistrations.Web.ViewModels
{
    public class InvitationsViewModel
    {
        public string SortColumn { get; set; }

        public string SortDirection { get; set; }

        public List<InvitationViewModel> Invitations { get; set; }
    }
}
