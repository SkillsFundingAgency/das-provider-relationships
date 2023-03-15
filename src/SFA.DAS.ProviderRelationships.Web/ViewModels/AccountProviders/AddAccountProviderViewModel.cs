using SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders
{
    public class AddAccountProviderViewModel : IValidatableObject
    {
        public ProviderDto Provider { get; set; }

        public long? AccountId { get; set; }
        public string AccountHashedId { get; set; }

        public Guid? UserRef { get; set; }
        
        [Required]
        public long? Ukprn { get; set; }
        
        [RegularExpression("Confirm|ReEnterUkprn", ErrorMessage = "Option required")]
        public string Choice { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Choice))
            {
                yield return new ValidationResult($"Select yes if you want to add {Provider.Name}.",  new[] { nameof(Choice) });
            }
        }
    }
}