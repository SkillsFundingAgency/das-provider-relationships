using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderRelationships.Validation;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels
{
    public class SearchProvidersViewModel 
    {
        [Required(ErrorMessage = ErrorMessages.InvalidUkprn)]
        [RegularExpression(@"[\d+]{8}", ErrorMessage = ErrorMessages.InvalidUkprn)]
        public string Ukprn { get; set; }
    }
}