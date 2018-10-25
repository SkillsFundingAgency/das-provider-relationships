using System.ComponentModel.DataAnnotations;
using MediatR;
using SFA.DAS.Authorization;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetAddedProviderQuery : IRequest<GetAddedProviderQueryResponse>, IAuthorizationContextMessage
    {
        [Required]
        public long? AccountId { get; set; }
        
        [Required]
        public int? AccountProviderId { get; set; }
    }
}