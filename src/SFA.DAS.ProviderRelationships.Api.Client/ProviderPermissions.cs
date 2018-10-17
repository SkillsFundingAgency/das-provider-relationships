using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public class ProviderPermissions : IProviderPermissions
    {
        public bool HasAtLeastOneRelationshipsWithThisPermission(long ukprn, ProviderPermissionType permission)
        {
            throw new System.NotImplementedException();
        }

        public IList<ProviderRelationship> ListRelationshipsWithThisPermission(long ukprn, ProviderPermissionType permission)
        {
            throw new System.NotImplementedException();
        }
    }
}