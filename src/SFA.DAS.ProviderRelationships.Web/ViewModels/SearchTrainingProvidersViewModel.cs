using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderRelationships.Application;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels
{
    public class SearchTrainingProvidersViewModel
    {
        [Required]
        public SearchTrainingProvidersQuery SearchTrainingProvidersQuery { get; set; }
    }
}