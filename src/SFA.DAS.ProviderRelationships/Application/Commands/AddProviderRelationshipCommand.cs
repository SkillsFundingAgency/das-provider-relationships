using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class AddProviderRelationshipCommand : IRequest
    {
        [Required]
        public long AccountLegalEntityId { get; set; }
        [Required]
        public long Ukprn { get; set; }
        
        // if provider already exists, we don't need the name. still required?
        [Required]
        public string ProviderName { get; set; }
    }
}