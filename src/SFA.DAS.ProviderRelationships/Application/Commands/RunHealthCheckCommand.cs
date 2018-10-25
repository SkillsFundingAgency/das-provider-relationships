using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using SFA.DAS.Authorization;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class RunHealthCheckCommand : IRequest, IAuthorizationContextMessage
    {
        [Required]
        public Guid? UserRef { get; set; }
    }
}