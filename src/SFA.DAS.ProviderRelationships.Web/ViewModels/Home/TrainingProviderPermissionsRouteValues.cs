using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.Home
{
    public class TrainingProviderPermissionsRouteValues : IAuthorizationContextModel
    {
        [Required]
        public long? AccountId { get; set; } 
    }
}