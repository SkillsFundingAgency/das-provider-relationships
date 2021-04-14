using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;

namespace SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders
{
    public class AccountProvidersRouteValues : IAuthorizationContextModel
    {
        [Required]
        public string AccountHashedId { get; set; }

        [Required]
        public long? AccountId { get; set; } 
    }
}