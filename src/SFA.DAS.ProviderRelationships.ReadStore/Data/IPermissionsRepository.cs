using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Document.Repository;
using SFA.DAS.ProviderRelationships.ReadStore.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Data
{
    internal interface IPermissionsRepository : IDocumentRepository<Permission>
    {
        Task Update(Permission permission, CancellationToken token = default);
    }
}