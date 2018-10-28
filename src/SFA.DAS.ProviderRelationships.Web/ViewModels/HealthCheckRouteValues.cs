using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels
{
    public class HealthCheckRouteValues : IAuthorizationContextModel
    {
        [Required]
        public Guid? UserRef { get; set; }
    }
}