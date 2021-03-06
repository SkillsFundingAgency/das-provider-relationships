﻿using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;

namespace SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviders
{
    public class InvitationAccountProviderRouteValues : IAuthorizationContextModel
    {
        [Required]
        public long? AccountId { get; set; }

        [Required]
        public Guid? CorrelationId { get; set; }

        [Required]
        public Guid? UserRef { get; set; }
    }
}