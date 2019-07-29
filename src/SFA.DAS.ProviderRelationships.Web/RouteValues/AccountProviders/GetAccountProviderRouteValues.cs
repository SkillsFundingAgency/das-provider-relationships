using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;

namespace SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders
{
    public class GetAccountProviderRouteValues : IAuthorizationContextModel
    {
        [Required]
        public string AccountHashedId { get; set; }

        [Required]
        public long? AccountId { get; set; }

        [Required]
        public long? AccountProviderId { get; set; }
    }
}