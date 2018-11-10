using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.Providers
{
    public class ProvidersRouteValues : IAuthorizationContextModel
    {
        [Required]
        public long? AccountId { get; set; } 
    }
}