using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public class ProviderPermissions : IProviderPermissions
    {
        public bool HasAtLeastOneRelationshipsWithThisPermission(long ukprn, PermissionEnumDto permission)
        {
            throw new System.NotImplementedException();
        }

        public IList<ProviderRelationshipDto> ListRelationshipsWithThisPermission(long ukprn, PermissionEnumDto permission)
        {
            throw new System.NotImplementedException();
        }
    }
}