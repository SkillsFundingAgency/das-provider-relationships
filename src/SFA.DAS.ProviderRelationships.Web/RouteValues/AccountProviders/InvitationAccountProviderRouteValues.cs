using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders
{
    public class InvitationAccountProviderRouteValues
    {
        [Required]
        public long? AccountId { get; set; }

        [Required]
        public Guid? CorrelationId { get; set; }

        [Required]
        public Guid? UserRef { get; set; }
    }
}