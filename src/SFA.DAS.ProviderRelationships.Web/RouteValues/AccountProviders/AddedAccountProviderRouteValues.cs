using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders
{
    public class AddedAccountProviderRouteValues : IAuthorizationContextModel
    {
        [Required]
        public long? AccountId { get; set; }

        [Required]
        public long? AccountProviderId { get; set; }
    }
}