using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.ReadStore.Models;
using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.Api.Client.Application
{
    public interface IProviderRelationshipService
    {
        Task<bool> HasRelationshipWithPermission(long ukPrn, PermissionEnumDto permission, CancellationToken cancellationToken);
        Task<IEnumerable<ProviderPermissions>> ListRelationshipsWithPermission(long ukPrn, PermissionEnumDto permission, CancellationToken cancellationToken);
    }
}