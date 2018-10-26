using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels
{
    public class AddProviderParameters
    {
        [Required]
        public long? Ukprn { get; set; }
    }
}