using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.ProviderRelationships.Web.RouteValues.HealthCheck
{
    public class HealthCheckRouteValues : IAuthorizationContextModel
    {
        [Required]
        public Guid? UserRef { get; set; }
    }
}