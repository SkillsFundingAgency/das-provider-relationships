using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class GetProviderQuery : IRequest<GetProviderQueryResponse>
    {
        [Required]
        public long? Ukprn { get; set; }
    }
}