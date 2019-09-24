using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.ProviderRegistrations.Web.Models
{
    public class AddDraftApprenticeshipViewModel : DraftApprenticeshipViewModel, IAuthorizationContextModel
    {
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }
        
        public long? AccountLegalEntityId { get; set; }
    }
}