using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRelationships.Web.RouteValues.HealthCheck
{
    public class HealthCheckRouteValues
    {
        [Required]
        public Guid? UserRef { get; set; }
    }
}