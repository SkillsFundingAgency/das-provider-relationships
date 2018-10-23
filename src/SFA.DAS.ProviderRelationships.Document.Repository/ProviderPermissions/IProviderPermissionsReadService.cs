using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Document.Model;

namespace SFA.DAS.ProviderRelationships.Document.Repository.ProviderPermissions
{
    public interface IProviderPermissionsReadService
    {
        Task<bool> Any(Expression<Func<ProviderRelationship, bool>> predicate);
    }
}
