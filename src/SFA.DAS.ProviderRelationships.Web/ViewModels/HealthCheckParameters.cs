using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels
{
    public class HealthCheckParameters : IAuthorizationContextMessage
    {
        [Required]
        public Guid? UserRef { get; set; }
    }
}