using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class AddOrUpdateUserCommand : IRequest
    {
        [Required]
        public Guid? Ref { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
    }
}