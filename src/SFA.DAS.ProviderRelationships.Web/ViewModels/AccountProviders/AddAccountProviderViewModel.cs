using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;
using SFA.DAS.ProviderRelationships.Application.Queries.GetProviderToAdd.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders
{
    public class AddAccountProviderViewModel : IAuthorizationContextModel
    {
        public ProviderDto Provider { get; set; }

        [Required]
        public long? AccountId { get; set; }

        [Required]
        public Guid? UserRef { get; set; }
        
        [Required]
        public long? Ukprn { get; set; }

        [Required(ErrorMessage = "Option required")]
        [RegularExpression("Confirm|ReEnterUkprn", ErrorMessage = "Option required")]
        public string Choice { get; set; }
    }
}