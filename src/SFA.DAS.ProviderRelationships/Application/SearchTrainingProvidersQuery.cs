using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application
{
    public class SearchTrainingProvidersQuery : IRequest<SearchTrainingProvidersQueryResponse>
    {
        [Required(ErrorMessage = ErrorMessages.InvalidUkprn)]
        [RegularExpression(@"[\d+]{8}", ErrorMessage = ErrorMessages.InvalidUkprn)]
        public string Ukprn { get; set; }
    }
}