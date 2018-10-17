using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public interface IProviderPermissions
    {
        bool HasAtLeastOneRelationshipsWithThisPermission(long ukprn, PermissionEnumDto permission);
        IList<ProviderRelationshipDto> ListRelationshipsWithThisPermission(long ukprn, PermissionEnumDto permission);
    }
}