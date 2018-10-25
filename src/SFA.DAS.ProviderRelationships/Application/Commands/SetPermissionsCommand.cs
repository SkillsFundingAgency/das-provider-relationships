using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediatR;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    // where live? name?
    public class Permission
    {
        [Required]
        public PermissionType Type { get; set; }
        [Required]
        public bool Granted { get; set; }
    }

    public class SetPermissionsCommand : IRequest
    {
        [Required]
        public long AccountLegalEntityId { get; set; }
        [Required]
        public long Ukprn { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public Guid UserRef { get; set; }

        [Required]
        public IEnumerable<Permission> Permissions { get; set; }
    }
}
