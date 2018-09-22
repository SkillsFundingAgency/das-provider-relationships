using System.ComponentModel.DataAnnotations;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application
{
    public class SearchTrainingProvidersQuery : IRequest<SearchTrainingProvidersQueryResponse>
    {
        [Required(ErrorMessage = "You must enter a valid UKPRN")]
        [RegularExpression(@"[\d+]{8}", ErrorMessage = "You must enter a valid UKPRN")]
        public string Ukprn { get; set; }
    }
}