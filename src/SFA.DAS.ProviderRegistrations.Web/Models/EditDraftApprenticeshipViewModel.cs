using System;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.ProviderRegistrations.Web.Models
{
    public class EditDraftApprenticeshipViewModel : DraftApprenticeshipViewModel, IAuthorizationContextModel
    {
        public EditDraftApprenticeshipViewModel()
        {
        }

        public string DraftApprenticeshipHashedId { get; set; }
        public long? DraftApprenticeshipId { get; set; }
    }
}