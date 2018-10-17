using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderRelationships.Application;
using SFA.DAS.ProviderRelationships.Application.Queries;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels
{
    public class SearchProvidersViewModel
    {
        [Required]
        public SearchProvidersQuery SearchProvidersQuery { get; set; }
    }
}