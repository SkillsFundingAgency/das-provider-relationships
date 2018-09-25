using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderRelationships.Application;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels
{
    public class SearchProvidersViewModel
    {
        [Required]
        public SearchProvidersQuery SearchProvidersQuery { get; set; }
    }
}