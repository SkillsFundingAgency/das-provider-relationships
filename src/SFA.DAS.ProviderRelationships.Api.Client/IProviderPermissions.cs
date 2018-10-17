using System.Collections;
using System.Collections.Generic;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public interface IProviderPermissions
    {
        bool HasAtLeastOneRelationshipsWithThisPermission(long ukprn, ProviderPermissionType permission);
        IList<ProviderRelationship> ListRelationshipsWithThisPermission(long ukprn, ProviderPermissionType permission);
    }
}