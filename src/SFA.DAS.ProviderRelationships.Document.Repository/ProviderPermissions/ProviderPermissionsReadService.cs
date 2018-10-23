using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Document.Model;
using SFA.DAS.ProviderRelationships.Document.Repository.CosmosDb;

namespace SFA.DAS.ProviderRelationships.Document.Repository.ProviderPermissions
{
    public class ProviderPermissionsReadService :  IProviderPermissionsReadService
    {
        private readonly CosmosDbClient<ProviderRelationship> _dbClient;
        private readonly string _collection;

        public ProviderPermissionsReadService(CosmosDbClient<ProviderRelationship> dbClient, string collection)
        {
            _dbClient = dbClient;
            _collection = collection;
        }

        public async Task<bool> Any(Expression<Func<ProviderRelationship, bool>> predicate)
        {
            var x = await _dbClient.Search(_collection, predicate);
            return x.Any();
        }
    }
}
