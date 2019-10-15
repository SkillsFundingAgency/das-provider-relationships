using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRegistrations.Web.ViewModels
{
    public class NewEmployerUserViewModel
    {
        [Display(Name = "Employer organisation")]
        [MaxLength(100)]
        [Required]
        public string EmployerOrganisation { get; set; }

        [Display(Name = "Employer first name")]
        [MaxLength(100)]
        [Required]
        public string EmployerFirstName { get; set; }

        [Display(Name = "Employer last name")]
        [MaxLength(100)]
        [Required]
        public string EmployerLastName { get; set; }

        [Display(Name = "Employer email address")]
        [MaxLength(100)]
        [Required]
        [EmailAddress]
        public string EmployerEmailAddress { get; set; }

        [Display(Name = "Provider email address")]
        [MaxLength(100)]
        public string ProviderEmailAddress { get; set; }

        [Display(Name = "Send copy of email")]
        public bool CopyEmailToProvider { get; set; }
    }
}