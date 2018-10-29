using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels
{
    public class AlreadyAddedProviderRouteValues : IAuthorizationContextModel
    {
        [Required]
        public long? AccountId { get; set; }
        
        [Required]
        public int? AccountProviderId { get; set; }
    }
}