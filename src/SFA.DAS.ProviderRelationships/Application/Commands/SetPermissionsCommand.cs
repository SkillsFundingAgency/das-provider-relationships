using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    // where live? name?
    public class Permission
    {
        public PermissionType Type { get; set; }
        public bool Granted { get; set; }
    }

    public class SetPermissionsCommand : IRequest
    {
        public long AccountLegalEntityId { get; set; }
        public long Ukprn { get; set; }
        public string UserName { get; set; }
        public Guid UserRef { get; set; }

        public IEnumerable<Permission> Permissions { get; set; }
    }
}
