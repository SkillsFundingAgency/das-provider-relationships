using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.Providers
{
    public class AddedProviderRouteValues : IAuthorizationContextModel
    {
        [Required]
        public long? AccountId { get; set; }
        
        [Required]
        public int? AccountProviderId { get; set; }
    }
}