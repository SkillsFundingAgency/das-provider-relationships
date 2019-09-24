using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ProviderRegistrations.Web.Models
{
    public class DraftApprenticeshipViewModel
    {
        public long ProviderId { get; set; }
        public string CohortReference { get; set; }
        public long? CohortId { get; set; }

        public Guid? ReservationId { get; set; }

        [Display(Name = "Employer")]
        [MaxLength(100)]
        public string Employer { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Unique Learner Number (ULN)")]
        public string Uln { get; set; }

        public string CourseCode { get; set; }

        [Display(Name = "Total agreed apprenticeship price (excluding VAT)")]
        public int? Cost { get; set; }

        [Display(Name = "Reference (optional)")]
        public string Reference { get; set; }
    }
}