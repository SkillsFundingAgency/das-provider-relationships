using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRelationships.Types.Models
{
    public enum Operation : short
    {
        [Display(Name = "Add apprentice records")] CreateCohort = 0,
        [Display(Name = "Recruit apprentices")] Recruitment = 1
    }
}