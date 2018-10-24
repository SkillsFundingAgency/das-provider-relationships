using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using SFA.DAS.Authorization;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class AddAccountProviderCommand : IRequest<int>, IAuthorizationContextMessage
    {
        [Required]
        public long? AccountId { get; set; }

        [Required]
        public Guid? UserRef { get; set; }
        
        [Required]
        public long? Ukprn { get; set; }
    }
}