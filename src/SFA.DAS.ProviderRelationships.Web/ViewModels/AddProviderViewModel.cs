﻿using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels
{
    public class AddProviderViewModel : IAuthorizationContextMessage
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