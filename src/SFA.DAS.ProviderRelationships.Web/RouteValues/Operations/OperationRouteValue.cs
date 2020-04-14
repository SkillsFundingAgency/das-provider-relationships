using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.Authorization;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Web.RouteValues.Operations
{
    public class OperationRouteValue : IAuthorizationContextModel
    {
        [Required]
        public long? AccountId { get; set; }

        [Required]
        public long? AccountProviderId { get; set; }

        [Required]
        public long? AccountLegalEntityId { get; set; }

        [Required]
        public short OperationId { get; set; }

        public bool? IsEnabled { get; set; }

        public bool IsEditMode { get; set; }       

        public OperationRouteValue()
        {
            OperationId = (short)Operation.NotSet;
        }        
    }
}